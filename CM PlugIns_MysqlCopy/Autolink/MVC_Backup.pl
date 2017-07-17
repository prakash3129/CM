use strict;
use Date::Calc qw(Delta_Days);
use File::Copy;
use DBI;
use DBD::mysqlPP;

my ($Second, $Minute, $Hour, $Day, $Month, $Year, $WeekDay, $DayOfYear, $IsDST) = localtime(time);
$Year +=1900;
$Month++;
my @today=($Year,$Month,$Day);
$Month = '0'.$Month if(length($Month)==1);
$Day = '0'.$Day if(length($Day)==1);
my $backup_folder_name = "D:\\CMBackup\\COMMON\\MVC_$Year\_$Month\_$Day";
mkdir "$backup_folder_name", 0777 unless -d "$backup_folder_name";
my $backup_folder_name_back = "D:\\CMBackup\\PROJECT\\";
print "backup_folder_name :: $backup_folder_name\n";
my $folder_name = $ARGV[0];

my $dbh = &dbconnection();
my ($cm_array_ref,$project_spec_array_ref,$match_file_regex) = &input_datails();
my @cm_array = @$cm_array_ref;
my @project_spec_array = @$project_spec_array_ref;
print "match_file_regex :: $match_file_regex\n";#exit;

my $folder_file_name_ref = &search_folder($folder_name);
my @folder_file_name_array = @$folder_file_name_ref;
my %hash_project_check;
foreach my $file_name_temp(@folder_file_name_array)
{
	my $file_name = $folder_name.'/'.$file_name_temp;
	my @first_stat = stat($file_name);
	my ($sec,$min,$hour,$mday,$mon,$year,$wday,$yday,$isdst) = localtime($first_stat[9]);
	$year +=1900;
	$mon++;
	my @fileday =($year,$mon,$mday);
	my $days_dif = Delta_Days(@fileday, @today);
	if($days_dif == 0)
	{
		$file_name_temp =~ s/\.[a-z]+?$//igs;
		if($file_name_temp =~ m/([^<]*?)_/is )
		{
			my $project_id = $1;
			if($project_id =~ m/^\s*c\s*$/is)
			{
				my $dest_file_name = $backup_folder_name.'/'.$file_name_temp.'.sql';
				system("mysqldump -h 172.27.138.181 -u root -pMeritGroup123 mvc $file_name_temp > $dest_file_name");
			}
			elsif(length($project_id ) == 9)
			{
				if($hash_project_check{$project_id})
				{
					next;
				}
				else
				{
					my $backup_folder_name = $backup_folder_name_back.$project_id;
					mkdir "$backup_folder_name", 0777 unless -d "$backup_folder_name";
					foreach my $part_file_array(@project_spec_array)
					{
						my $file_name_temp = $project_id.'_'.$part_file_array;
						my $dest_file_name = $backup_folder_name.'/'.$file_name_temp.'.sql';
						system("mysqldump -h 172.27.138.181 -u root -pMeritGroup123 mvc $file_name_temp > $dest_file_name");
					}
				}
			}
			else
			{
				my $dest_file_name = $backup_folder_name.'/'.$file_name_temp.'.sql';
				system("mysqldump -h 172.27.138.181 -u root -pMeritGroup123 mvc $file_name_temp > $dest_file_name");
			}
			$hash_project_check{$project_id} = 1;
		}
		
	}
}
sub search_folder()
{
	my $path = shift;
	# print "path :: $path \n";
	my @folder_list_array;
	opendir(DIR, $path);
	while (my $file = readdir(DIR)) 
	{
		push(@folder_list_array,$file) if ($file =~ m/$match_file_regex/is );
		# print "file :: $file\n";
	}
	
	closedir(DIR);
	return(\@folder_list_array)
}

sub dbconnection()
{
	my $db_host = '172.27.138.181';
	my $db_user = 'root';
	my $db_pass = 'MeritGroup123';
	my $dbh = DBI->connect("DBI:mysqlPP:database=MVC;host=$db_host","$db_user","$db_pass",{mysql_enable_utf8 => 1});
	return $dbh;
}

sub input_datails()
{
	my $query = "select picklistcategory,picklistvalue from c_picklists
					where picklistcategory='CM TABLES' or picklistcategory='PROJECT SPECIFIC TABLES'
					order by picklistcategory";
	
	my $sth = $dbh->prepare($query);
	my ($picklistcategory,$picklistvalue,@cm_array,@project_spec_array,$match_regex);
	$match_regex ='(';
	if($sth->execute())
	{
		while(my @record = $sth->fetchrow)
		{
			$picklistcategory    = &trim($record[0]);
			$picklistvalue = &trim($record[1]);
			if($picklistcategory eq "CM TABLES")
			{
				push(@cm_array,$picklistvalue);
				$match_regex = $match_regex.'^'.$picklistvalue.'\.ibd$|';
			}
			elsif($picklistcategory eq "PROJECT SPECIFIC TABLES")
			{
				push(@project_spec_array,$picklistvalue);
				$match_regex = $match_regex.$picklistvalue.'\.ibd$|';
			}
		}
		$sth->finish();
		$match_regex =~ s/\|\s*$/)/igs;
		
	}
	else
	{
		open fh,">>RetrieveID_INFO_Error.txt";
		print fh "$query query get following error $DBI::errstr\n";
		close fh;
		# sleep ;
		print "Query Failed Reconnect\n";
		$dbh=&dbconnection();
	}
	# print "$company_name,$company_country,$master_id\n";exit;
	
	return (\@cm_array,\@project_spec_array,$match_regex);
}

sub trim()
{
	my $text = shift;
	# $text =decode_entities($text);
	$text =~ s/\s+/ /igs;
	$text =~ s/^\s*|\s*$//igs;
	return $text;
}