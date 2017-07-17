use strict;
use HTTP::Cookies;
use LWP::UserAgent;
use HTML::Entities;
use DBI;
use DBD::mysqlPP;
use utf8;
use URI::URL;
use Unicode::Normalize;
use Sys::Hostname;
use Win32::Process::Info;
use Net::Ping;
use Socket;
    
my $dot_net_communication = 'Update_Status.ini';
my $log_file = 'Bridge.txt';

my($db_host,$db_user,$db_pass,$project_id,$agent_name,$username,$password,$complete_contact_status,$flag) = @ARGV;


my %picklist_detail;
my $dbh = &dbconnection();
&picklist_details();
my $excep_folder = $picklist_detail{'Path_DataMiner'};
$excep_folder = $excep_folder .'\\' unless($excep_folder =~ m/\\\s*$/is);
my $excep_folder_name = $excep_folder.$agent_name.'_'.$project_id.'_Log.txt';

# print "excep_folder_name :: $excep_folder_name\n"; exit;

$password =~ s/\&/\%26/igs;
$password =~ s/\!/\%21/igs;
my($insert_query_column,$Search_KeywordList,@exculsion_list,$MaxContacts,$takencontacts,$coninsertquery,$in_jobtitle,@array_in_jobtitle,$ping_count,$date_column,$email_dupecheck_project,$emailfinder_service,$companyinfo,$thirdconnection_process);
$date_column = 'TR_DATECALLED' if(uc($flag) eq 'TR');
$date_column = 'WR_DATE_OF_PROCESS' if(uc($flag) eq 'WR');
my %hash_country_pick_list = ('denmark'=>'dk', 'netherland'=>'nl', 'uk'=>'gb', 'afghanistan'=>'af', 'albania'=>'al', 'algeria'=>'dz', 'argentina'=>'ar', 'australia'=>'au', 'austria'=>'at', 'bahrain'=>'bh', 'bangladesh'=>'bd', 'belgium'=>'be', 'bolivia'=>'bo', 'bosnia and herzegovina'=>'ba', 'brazil'=>'br', 'bulgaria'=>'bg', 'canada'=>'ca', 'chile'=>'cl', 'china'=>'cn', 'colombia'=>'co', 'costa rica'=>'cr', 'croatia'=>'hr', 'cyprus'=>'cy', 'czech republic'=>'cz', 'dominican republic'=>'do', 'ecuador'=>'ec', 'egypt'=>'eg', 'el salvador'=>'sv', 'estonia'=>'ee', 'finland'=>'fi', 'france'=>'fr', 'germany'=>'de', 'ghana'=>'gh', 'greece'=>'gr', 'guatemala'=>'gt', 'hong kong'=>'hk', 'hungary'=>'hu', 'iceland'=>'is', 'india'=>'in', 'indonesia'=>'id', 'iran'=>'ir', 'ireland'=>'ie', 'israel'=>'il', 'italy'=>'it', 'jamaica'=>'jm', 'japan'=>'jp', 'jordan'=>'jo', 'kazakhstan'=>'kz', 'kenya'=>'ke', 'korea'=>'kr', 'kuwait'=>'kw', 'latvia'=>'lv', 'lebanon'=>'lb', 'lithuania'=>'lt', 'luxembourg'=>'lu', 'macedonia'=>'mk', 'malaysia'=>'my', 'malta'=>'mt', 'mauritius'=>'mu', 'mexico'=>'mx', 'morocco'=>'ma', 'nepal'=>'np', 'netherlands'=>'nl', 'new zealand'=>'nz', 'nigeria'=>'ng', 'norway'=>'no', 'oman'=>'om', 'pakistan'=>'pk', 'panama'=>'pa', 'peru'=>'pe', 'philippines'=>'ph', 'poland'=>'pl', 'portugal'=>'pt', 'puerto rico'=>'pr', 'qatar'=>'qa', 'romania'=>'ro', 'russian federation'=>'ru', 'saudi arabia'=>'sa', 'singapore'=>'sg', 'slovak republic'=>'sk', 'slovenia'=>'si', 'south africa'=>'za', 'spain'=>'es', 'sri lanka'=>'lk', 'sweden'=>'se', 'switzerland'=>'ch', 'taiwan'=>'tw', 'tanzania'=>'tz', 'thailand'=>'th', 'trinidad and tobago'=>'tt', 'tunisia'=>'tn', 'turkey'=>'tr', 'uganda'=>'ug', 'ukraine'=>'ua', 'united arab emirates'=>'ae', 'united kingdom'=>'gb', 'united states'=>'us', 'uruguay'=>'uy', 'venezuela'=>'ve', 'vietnam'=>'vn', 'zimbabwe'=>'zw');


my $multiple_company_flag = 0;
my $company_search_url_part;
my ($coninsertquery_temp,$select_query,$injobtitle,$OutCon_LastName,$OutCon_FirstName,$MaxContacts_Temp,$exculsion_list_ref) = &columnname_retrive($project_id);
my $common_word_ref = &common_word_datails();
my @common_word_list = @$common_word_ref;
my @exculsion_list = @$exculsion_list_ref;
my (%domain_company,%domain_company_website,%DM_CompanyType,%DM_CompanyHeadquarters,%DM_CompanySize,%DM_CompanyFounded,%DM_CompanySpecialties,%DM_CompanyProfileLink,%DM_CompanyFollowers,%DM_CompanyDescription,%DM_EmailScore,%DM_CompanyIndustry);

my $ua=LWP::UserAgent->new(ssl_opts => { verify_hostname => 0 }, show_progress=>1);
$ua->agent("Mozilla/5.0 (Windows NT 5.1; rv:18.0) Gecko/20100101 Firefox/18.0");
$ua->timeout(90);
my $cookie=HTTP::Cookies->new(file=>$0."_cookie.txt", autosave=>1);
$ua->cookie_jar($cookie);
&internetcheck();
my $ping_count=0;


my $home_url = 'https://www.linkedin.com/uas/login';
my $home_content = &getcontent($home_url,$ping_count,$ua,$cookie);
my $ajax_value 		= $1 if($home_content =~ m/ajax\:([^>]*?)\"/is); #1056255767199365408
my $loginkey  = $1 if($home_content =~ m/loginCsrfParam\"\s*value\=\"([^>]*?)\"/is); #f5b31f62-9c57-4d16-8359-68ad7f0915de

my $post_url = 'https://www.linkedin.com/uas/login-submit';
my $post_content = 'isJsEnabled=true&source_app=&tryCount=&clickedSuggestion=false&session_key='.$username.'&session_password='.$password.'&signin=Sign%20In&session_redirect=&trk=&loginCsrfParam='.$loginkey.'&csrfToken=ajax%3A'.$ajax_value.'&sourceAlias=0_7r5yezRXCiA_H0CRD8sf6DhOjTKUNps5xGTqeX8EEoi&client_ts=1446112709122&client_r='.$username.'%3A649556529%3A183502753%3A862761727&client_output=-865658456&client_n=649556529%3A183502753%3A862761727&client_v=1.0.1';

my $loginecontent = &postcontent($post_url,$post_content,$home_url,$ping_count,$ua,$cookie);

unless($loginecontent =~ m/<a[^<]*?href\=\"([^<]*?)\"[^>]*?>\s*Sign\s*Out\s*</is)
{
	print "Invalid username or Password\n";
	open(FH,">$log_file");
	print FH "LoginStatus:~Invalid login\n";
	close FH;
	exit;
}

unless($loginecontent =~ m/https:\/\/www\.linkedin\.com\/\?trk\=prem_logo/is)
{
	print "Please give me premium account\n";
	open(FH,">$log_file");
	print FH "LoginStatus:~No Premium\n";
	close FH;
	if($loginecontent =~ m/<a[^<]*?href\=\"([^<]*?)\"[^>]*?>\s*Sign\s*Out\s*</is)
	{
		my $signout_url = $1;
		$signout_url =~ s/amp\;//igs;
		my $home_content = &getcontent($signout_url,$ping_count,$ua,$cookie);
		open sr,">signout.html";
		print sr $home_content;
		close sr;
	}
	exit;
}
my $host_name = hostname();
my $SCRAPPER_VERSION = 'DEV';
# my $SCRAPPER_VERSION = '1.0';
my ($input_datails,$inloacation);

my ($incompany,$masterid,$inloacation_temp) = &input_datails($select_query);
my $log_session_id = &get_session_id();

nextrecord:
$inloacation = $inloacation_temp if($inloacation_temp ne "");
my $takencontacts=0;
# &check_cm_status();
my ($third_connection_count,$third_connection_skipped_count,$pages_navigated,$unmatched_count,$duplicate_count);
$third_connection_count = 0;$third_connection_skipped_count = 0;$pages_navigated = 1;$unmatched_count = 0;$duplicate_count = 0;
my $coninsertquery = $coninsertquery_temp;
my $latest_log_insert_id;
if($incompany ne "")
{
	my ($search_company_name,$Country_Abbr,$company_id,$search_result);
	$search_company_name = $incompany; 
	$Country_Abbr = $inloacation; 
	
	
	if($multiple_company_flag == 0)
	{
		$Country_Abbr = $hash_country_pick_list{lc($Country_Abbr)};
	}
	
	
	open(FH,">$log_file");
	print FH "Current CompanyName:~$incompany,$inloacation\n";
	print FH "Current CompanyID:~$masterid\n";
	close FH;
	my ($hash_linked_name_ref,$exist_count) = &duplicate_check($complete_contact_status,$project_id,$OutCon_FirstName,$OutCon_LastName,$masterid);
	my $MaxContacts = $MaxContacts_Temp - $exist_count;
	my %hash_linked_name = %$hash_linked_name_ref;
	my @array_exist_dbname = values %hash_linked_name;	
	my $search_url;
	my (@array_linkedin_member,@array_linkedin_member_content,$job_title_process_count);
	
	my $job_title_length = $#array_in_jobtitle+1;
	$job_title_process_count = 0;
	foreach $in_jobtitle (@array_in_jobtitle)
	{
		$job_title_process_count++;
		if($multiple_company_flag)
		{
			$search_url = 'https://www.linkedin.com/vsearch/p?title='.$in_jobtitle.'&company='.$search_company_name.'&openAdvancedForm=true&titleScope=C&companyScope=C'.$company_search_url_part.'&orig=MDYS';;
		}
		elsif($Country_Abbr)
		{
			
			$search_url = 'https://www.linkedin.com/vsearch/p?title='.$in_jobtitle.'&company='.$search_company_name.'&openAdvancedForm=true&titleScope=C&companyScope=C&locationType=I&countryCode='.$Country_Abbr.'&orig=MDYS';
		}
		else
		{
			$search_url = 'https://www.linkedin.com/vsearch/p?title='.$in_jobtitle.'&company='.$search_company_name.'&openAdvancedForm=true&titleScope=C&companyScope=C&locationType=Y&orig=MDYS';
		}
		my $search_cont = &getcontent($search_url,$ping_count,$ua,$cookie);
		open sr,">SearchResult.html";
		print sr $search_cont;
		close sr;
		if($search_cont =~ m/\"resultCount\"\:([^<]*?)\,\"/is)
		{
			$search_result = $1;
		}
		 $latest_log_insert_id = &insert_log($project_id,$masterid,$search_url,$username,$search_result,$agent_name,$flag,$host_name,$SCRAPPER_VERSION,$log_session_id);
		 open net,">$dot_net_communication";
		 print net "1";
		 close net;
		
		Nextpage:
		while($search_cont =~ m/\{\"person\"\:([\w\W]*?)\s*\}\}\,/igs)
		{
			my $single_person_cont = $1;
			my $P_link;
			if($single_person_cont=~m/(?:link_nprofile_view_3|link_nprofile_view_headless)\"\:\"([^>]*?)\"\,/is)
			{
				$P_link = &urlcheck($1);
				
			}
			if($single_person_cont =~ m/fmt_name\"\:\"([^>]*?)\"\,/is)
			{
				my ($position_company,$fullname,$location,$industry,$firstname,$lastname,$company,$pro_job_title,$company_match_flag);
				next unless($single_person_cont =~ m/\"Current\"/is);
				$company_match_flag = 0;
				while($single_person_cont =~ m/\"Current\"\,\"heading\"\:\"([\w\W]*?)\"\,\"/igs)
				{
					$position_company = $1;
					$position_company = &trim_json($position_company);
					$position_company = &trim($position_company);
					if($position_company =~ m/([^<]*?)\s+at\s+([^>]*?)$/is)
					{
						$pro_job_title = $1;
						$company = $2;
					}
					elsif($position_company =~ m/([^<]*?)\s+\-\s+([^>]*?)$/is)
					{
						$pro_job_title = $1;
						$company = $2;
					}
					$company_match_flag = &company_match($incompany,$company);
					if($company_match_flag >=1 )
					{
						last;
					}
					
				}
				while($single_person_cont =~ m/\"Current\"\,\"bodyList\"\:\s*\[\"([\w\W]*?)\"\]/igs)
				{
					$position_company = $1;
					$position_company = &trim_json($position_company);
					$position_company = &trim($position_company);
					if($position_company =~ m/([^<]*?)\s+at\s+([^>]*?)$/is)
					{
						$pro_job_title = $1;
						$company = $2;
					}
					elsif($position_company =~ m/([^<]*?)\s+\-\s+([^>]*?)$/is)
					{
						$pro_job_title = $1;
						$company = $2;
					}
					$company_match_flag = &company_match($incompany,$company);
					if($company_match_flag >=1 )
					{
						last;
					}
				}
				if($company_match_flag ==0)
				{
					$unmatched_count++;
					next;
				}
				if($single_person_cont =~ m/fmt_name\"\:\"([^>]*?)\"\,/is)
				{
					$fullname = $1;
					$fullname = &trim($fullname);
				}
				if($single_person_cont =~ m/fmt_location\"\:\"([^>]*?)\"\,/is)
				{
					$location = $1;
					$location = &trim($location);
				}
				if($single_person_cont =~ m/fmt_industry\"\:\"([^>]*?)\"\,/is)
				{
					$industry = $1;
					$industry = &trim($industry);
				}
				if($single_person_cont =~ m/fnamep\"\:\"([^>]*?)\"\,/is)
				{
					$firstname = $1;
					$firstname = &trim($firstname);
				}
				if($single_person_cont =~ m/lnamep\"\:\"([^>]*?)\"\,/is)
				{
					$lastname = $1;
					$lastname = &trim($lastname);
				}
				
				$location =~ s/\s+area\s*\,/\,/igs;
				my ($con_city,$con_country);
				if($location =~ m/([^<]*?)\,([^>]*?)$/is)
				{
					$con_city = &trim($1);
					$con_country = &trim($2);
					
				}
				else
				{
					$con_country = $location;
				}
				$con_city =~ s/\s+area\W*\s*$//igs;
				$con_country =~ s/\s+area\W*\s*$//igs;
				$lastname =~ s/\,[^>]*?$//igs;
				my $concat_name = lc($firstname.$lastname);
				# print "concat_name :: $concat_name";<STDIN>;
				my @db_matched = grep { $_ eq $concat_name } @array_exist_dbname;
				my @exe_matched = grep { $pro_job_title =~ m/\b$_\b/i } @exculsion_list;
				if(@exe_matched)
				{
					$unmatched_count++;
					next;
				}
				if(@db_matched)
				{
					$duplicate_count++;
					# next;
				}
				unless(@db_matched)
				{
					push(@array_exist_dbname,$concat_name);
					if(uc($companyinfo) eq 'Y')
					{
						unless($domain_company{lc($company)})
						{
							$company =~ s/amp\;//igs;
							$company =~ s/\,//igs;
							my $jsoncompanyurl = 'https://www.linkedin.com/ta/company?query='.$company;
							my $temp_Company = quotemeta($company);
							my $temp_industry = quotemeta($industry);
							my $jsoncompanycontent = &getcontent($jsoncompanyurl,$ping_count,$ua,$cookie);
							$jsoncompanycontent =~ s/amp\;//igs;
							$jsoncompanycontent =~ s/\,//igs;
							$temp_Company =~ s/amp\;//igs;
							if($jsoncompanycontent =~ m/\{\"displayName\"\:\"$temp_Company\"[^\}]+?\"url\"\:\"([^<]*?)\"/is)
							{
								my $linkedincompanyurl= $1;
								$DM_CompanyProfileLink{lc($company)} = $linkedincompanyurl;
								my $linkedincompanycontent = &getcontent($linkedincompanyurl,$ping_count,$ua,$cookie);
								open sr,">linkedincompanycontent.html";
								print sr $linkedincompanycontent;
								close sr;
								&Company_details_collect($linkedincompanycontent,$company);
								
											
							}
							else
							{
								$domain_company{lc($company)} = "-";
							}
						}
					}
					my $company_link =$domain_company{lc($company)};
					my $company_link_ori = $company_link;
					unless($company_link =~ m/^\s*\-\s*$/is)
					{
						my $u = URI::URL->new($company_link);
						$company_link = $u->host();
						$company_link =~ s/^\s*www\.//igs;
					}
					my $Contact_Region;
					if($con_country)
					{
						$Contact_Region = &region_details($con_country);
					}
					$lastname =~ s/\,[^>]*?$//igs;
					$company_link =~s/\'/''/igs;
					$company_link =~s/^\s*\-\s*$//igs;
					$company_link_ori =~s/\'/''/igs;
					$company_link_ori =~s/^\s*\-\s*$//igs;
					$lastname =~s/\'/''/igs;
					$firstname =~s/\'/''/igs;
					$pro_job_title =~s/\'/''/igs;
					$P_link =~s/\'/''/igs;
					$company =~s/\'/''/igs;
					$DM_CompanyType{lc($company)} =~s/\'/''/igs;
					$DM_CompanyHeadquarters{lc($company)} =~s/\'/''/igs;
					$DM_CompanySize{lc($company)} =~s/\'/''/igs;
					$DM_CompanyFounded{lc($company)} =~s/\'/''/igs;
					$DM_CompanySpecialties{lc($company)} =~s/\'/''/igs;
					$DM_CompanyProfileLink{lc($company)} =~s/\'/''/igs;
					$DM_CompanyFollowers{lc($company)} =~s/\'/''/igs;
					$DM_CompanyDescription{lc($company)} =~s/\'/''/igs;
					$DM_CompanyFollowers{lc($company)} =~s/\,//igs;
					$company = &trim($company);
					$pro_job_title = &trim($pro_job_title);
					$domain_company_website{lc($company)} = &trim($company_link_ori);
					$con_city =~s/\'/''/igs;
					$con_country =~s/\'/''/igs;
					$Contact_Region =~s/\'/''/igs;
					$DM_CompanyFollowers{lc($company)} = 'NULL'  if($DM_CompanyFollowers{lc($company)} eq "");
					$takencontacts++;
					
					$coninsertquery .= "(\'$firstname\',\'$lastname\',\'$pro_job_title\',\'$company\',\'$P_link\',now(),\'$agent_name\',\'$masterid\',now(),SOUNDEX(\'$firstname\'),SOUNDEX(\'$lastname\'),\'NEW\',now(),\'$agent_name\',1,'LS_STATUS','LS_STATUS',\'$industry\',\'$company_link_ori\',\'$DM_CompanyType{lc($company)}\',\'$DM_CompanyHeadquarters{lc($company)}\',\'$DM_CompanySize{lc($company)}\',\'$DM_CompanyFounded{lc($company)}\',\'$DM_CompanySpecialties{lc($company)}\',\'$DM_CompanyProfileLink{lc($company)}\',$DM_CompanyFollowers{lc($company)},\'$DM_CompanyDescription{lc($company)}\',\'$company_link\',\'$con_city\','',\'$con_country\',\'$Contact_Region\','DataMiner')\,";
					if( $takencontacts >= $MaxContacts )
					{
						$coninsertquery =~s/\,\s*$//igs;
						$coninsertquery .= ';';
						&insert_query($coninsertquery);
						goto companyconcomplete;
					}
				}
			}
			elsif($single_person_cont=~m/fmt_headline\"\:\"([\w\W]*?)\"\,/is) # Pattern match to check if member name displayed as "Linkedin Member".
			{
				my $Dum_des = $1;
				$third_connection_skipped_count++;
				$Dum_des =~ s/\\u[^>]*?\\u[^>]*?[a-z]+//igs;
				my $company_match_flag = 0;
				my ($pro_job_title,$company,$position_company);
				while($single_person_cont =~ m/\"Current\"\,\"heading\"\:\"([\w\W]*?)\"\,\"/igs)
				{
					$position_company = $1;
					$position_company = &trim_json($position_company);
					$position_company = &trim($position_company);
					if($position_company =~ m/([^<]*?)\s+at\s+([^>]*?)$/is)
					{
						$pro_job_title = $1;
						$company = $2;
					}
					elsif($position_company =~ m/([^<]*?)\s+\-\s+([^>]*?)$/is)
					{
						$pro_job_title = $1;
						$company = $2;
					}
					$company_match_flag = &company_match($incompany,$company);
					if($company_match_flag >=1 )
					{
						last;
					}
					
				}
				while($single_person_cont =~ m/\"Current\"\,\"bodyList\"\:\s*\[\"([\w\W]*?)\"\]/igs)
				{
					$position_company = $1;
					$position_company = &trim_json($position_company);
					$position_company = &trim($position_company);
					if($position_company =~ m/([^<]*?)\s+at\s+([^>]*?)$/is)
					{
						$pro_job_title = $1;
						$company = $2;
					}
					elsif($position_company =~ m/([^<]*?)\s+\-\s+([^>]*?)$/is)
					{
						$pro_job_title = $1;
						$company = $2;
					}
					$company_match_flag = &company_match($incompany,$company);
					if($company_match_flag >=1 )
					{
						last;
					}
				}
				if($company_match_flag >= 1)
				{
					push(@array_linkedin_member_content,$single_person_cont);
					$third_connection_skipped_count++;
				}
				else
				{
					$unmatched_count++;
				}
			}
		}

		if($search_cont=~m/nextPage[^>]*?pageURL\"\:\"([^>]*?)\"/is)
		{
			print ">>>>>>NEXT PAGE<<<<<<\n";
			my $next_link =("https://www.linkedin.com".$1);
			$next_link=~s/\\u002d/-/igs;
			$pages_navigated++;
			$search_cont = &getcontent($next_link,$ping_count,$ua,$cookie);
			eval{
			$search_cont = Encode::decode("UTF-8", $search_cont);
			};
			if ($@){
				open (CR,">>next_link.txt");
				print CR "$next_link\n";
				close CR;
			}
			
			goto Nextpage;
		}
		if($job_title_process_count < $job_title_length)
		{
			my $log_update_query = "update c_scrapper_log set MATCHED=$takencontacts,THIRD_CONNECTION_PROCESSED=0,THIRD_CONNECTION_SKIPPED=0,PAGES_NAVIGATED=$pages_navigated,UNMATCHED=0,DUPLICATE=0,STATUS=\'Researched\' ,END_TIME = now() where id =$latest_log_insert_id " ;
			&update_query($log_update_query) if($latest_log_insert_id);
		}
	}
		if(uc($thirdconnection_process) eq 'Y')
		{
			foreach my $single_person_cont (@array_linkedin_member_content)
			{
				
				if($single_person_cont=~m/\"send_inmail\"/is)
				{
					my ($position_company,$fullname,$location,$industry,$firstname,$lastname,$company,$pro_job_title,$company_match_flag);
					if($single_person_cont =~ m/link_createReferralProposal_1\"\:\"([^<]*?)\"/is)
					{
						my $Inmail_link = &urlcheck($1);
						my $Inmail_link_cont = &getcontent($Inmail_link,$ping_count,$ua,$cookie);
						if($Inmail_link_cont =~ m/\"recipientProfile\"\:\{\"([\w\W]*?)\}/is)
						{
							my $json_content = $1;
							if($json_content =~ m/\"firstName\"\:\"([^<]*?)\"/is)
							{
								$firstname = $1;
							}
							if($json_content =~ m/\"lastName\"\:\"([^<]*?)\"/is)
							{
								$lastname = $1;
							}
							if($firstname eq "" && $lastname eq "")
							{
								if($Inmail_link_cont =~ m/<h\d+[^<]*? class\=\"compose\-header\">\s*<label>\s*To\:\s*<\/label>([^<]*?)<\/h\d+>/is)
								{
									$fullname = &trim($1);
									if($fullname =~ m/([^>]*?)\s+(\S+)$/is)
									{
										$firstname = $1;
										$lastname = $2;
										if($firstname =~ m/(\bvan de\b|\bvan\b|\bde\b|\bda\b|\bal\b|\bla\b|\bvon\b|\bdu\b|\ble\b|\bles\b)/is)
										{
											my $match_key = $1;
											$lastname = $match_key.' '.$lastname;
											$firstname =~ s/\b$match_key\s*//igs;
											
										}
									}
								}
							}
						}
						
					}
					my $P_link;
					if($single_person_cont=~m/(?:link_nprofile_view_3|link_nprofile_view_headless)\"\:\"([^>]*?)\"\,/is)
					{
						$P_link = &urlcheck($1);
					}
					if($firstname eq "" && $lastname eq "")
					{
						push(@array_linkedin_member,$P_link);
						next;
					}
					$third_connection_count++;
					while($single_person_cont =~ m/\"Current\"\,\"heading\"\:\"([\w\W]*?)\"\,\"/igs)
					{
						$position_company = $1;
						$position_company = &trim_json($position_company);
						$position_company = &trim($position_company);
						if($position_company =~ m/([^<]*?)\s+at\s+([^>]*?)$/is)
						{
							$pro_job_title = $1;
							$company = $2;
						}
						elsif($position_company =~ m/([^<]*?)\s+\-\s+([^>]*?)$/is)
						{
							$pro_job_title = $1;
							$company = $2;
						}
						$company_match_flag = &company_match($incompany,$company);
						if($company_match_flag >=1 )
						{
							last;
						}
						
					}
					while($single_person_cont =~ m/\"Current\"\,\"bodyList\"\:\s*\[\"([\w\W]*?)\"\]/igs)
					{
						$position_company = $1;
						$position_company = &trim_json($position_company);
						$position_company = &trim($position_company);
						if($position_company =~ m/([^<]*?)\s+at\s+([^>]*?)$/is)
						{
							$pro_job_title = $1;
							$company = $2;
						}
						elsif($position_company =~ m/([^<]*?)\s+\-\s+([^>]*?)$/is)
						{
							$pro_job_title = $1;
							$company = $2;
						}
						$company_match_flag = &company_match($incompany,$company);
						if($company_match_flag >=1 )
						{
							last;
						}
					}
					if($single_person_cont =~ m/fmt_location\"\:\"([^>]*?)\"\,/is)
					{
						$location = $1;
						$location = &trim($location);
					}
					if($single_person_cont =~ m/fmt_industry\"\:\"([^>]*?)\"\,/is)
					{
						$industry = $1;
						$industry = &trim($industry);
					}
					$location =~ s/area\s*\,/\,/igs;
					my ($con_city,$con_country);
					if($location =~ m/([^<]*?)\,([^>]*?)$/is)
					{
						$con_city = &trim($1);
						$con_country = &trim($2);
					}
					else
					{
						$con_country = $location;
					}
					$lastname =~ s/\,[^>]*?$//igs;
					$con_city =~ s/\s+area\W*\s*$//igs;
					$con_country =~ s/\s+area\W*\s*$//igs;
					my $concat_name = lc($firstname.$lastname);
					# print "concat_name :: $concat_name";<STDIN>;
					my @db_matched = grep { $_ eq $concat_name } @array_exist_dbname;
					my @exe_matched = grep { $pro_job_title =~ m/\b$_\b/i } @exculsion_list;
					if(@exe_matched)
					{
						$unmatched_count++;
						next;
					}
					if(@db_matched)
					{
						$duplicate_count++;
						# next;
					}
					unless(@db_matched)
					{
						push(@array_exist_dbname,$concat_name);
						if(uc($companyinfo) eq 'Y')
						{
							unless($domain_company{lc($company)})
							{
								$company =~ s/amp\;//igs;
								$company =~ s/\,//igs;
								my $jsoncompanyurl = 'https://www.linkedin.com/ta/company?query='.$company;
								my $temp_Company = quotemeta($company);
								my $temp_industry = quotemeta($industry);
								my $jsoncompanycontent = &getcontent($jsoncompanyurl,$ping_count,$ua,$cookie);
								$jsoncompanycontent =~ s/amp\;//igs;
								$jsoncompanycontent =~ s/\,//igs;
								$temp_Company =~ s/amp\;//igs;
								if($jsoncompanycontent =~ m/\{\"displayName\"\:\"$temp_Company\"[^\}]+?\"url\"\:\"([^<]*?)\"/is)
								{
									my $linkedincompanyurl= $1;
									$DM_CompanyProfileLink{lc($company)} = $linkedincompanyurl;
									my $linkedincompanycontent = &getcontent($linkedincompanyurl,$ping_count,$ua,$cookie);
									&Company_details_collect($linkedincompanycontent,$company);
									
												
								}
								else
								{
									$domain_company{lc($company)} = "-";
								}
							}
						}
						my $company_link =$domain_company{lc($company)};
						my $company_link_ori = $company_link;
						unless($company_link =~ m/^\s*\-\s*$/is)
						{
							my $u = URI::URL->new($company_link);
							$company_link = $u->host();
							$company_link =~ s/^\s*www\.//igs;
						}
						my $Contact_Region;
						if($con_country)
						{
							$Contact_Region = &region_details($con_country);
						}
						$lastname =~ s/\,[^>]*?$//igs;
						$company_link =~s/\'/''/igs;
						$company_link =~s/^\s*\-\s*$//igs;
						$company_link_ori =~s/\'/''/igs;
						$company_link_ori =~s/^\s*\-\s*$//igs;
						$lastname =~s/\'/''/igs;
						$firstname =~s/\'/''/igs;
						$pro_job_title =~s/\'/''/igs;
						$P_link =~s/\'/''/igs;
						$company =~s/\'/''/igs;
						$DM_CompanyType{lc($company)} =~s/\'/''/igs;
						$DM_CompanyHeadquarters{lc($company)} =~s/\'/''/igs;
						$DM_CompanySize{lc($company)} =~s/\'/''/igs;
						$DM_CompanyFounded{lc($company)} =~s/\'/''/igs;
						$DM_CompanySpecialties{lc($company)} =~s/\'/''/igs;
						$DM_CompanyProfileLink{lc($company)} =~s/\'/''/igs;
						$DM_CompanyFollowers{lc($company)} =~s/\'/''/igs;
						$DM_CompanyFollowers{lc($company)} =~s/\,//igs;
						$DM_CompanyDescription{lc($company)} =~s/\'/''/igs;
						$company = &trim($company);
						$pro_job_title = &trim($pro_job_title);
						$domain_company_website{lc($company)} = &trim($company_link_ori);
						$con_city =~s/\'/''/igs;
						$con_country =~s/\'/''/igs;
						$Contact_Region =~s/\'/''/igs;
						$takencontacts++;
						$DM_CompanyFollowers{lc($company)} = 'NULL'  if($DM_CompanyFollowers{lc($company)} eq "");
												
						$coninsertquery .= "(\'$firstname\',\'$lastname\',\'$pro_job_title\',\'$company\',\'$P_link\',now(),\'$agent_name\',\'$masterid\',now(),SOUNDEX(\'$firstname\'),SOUNDEX(\'$lastname\'),\'NEW\',now(),\'$agent_name\',1,'LS_STATUS','LS_STATUS',\'$industry\',\'$company_link_ori\',\'$DM_CompanyType{lc($company)}\',\'$DM_CompanyHeadquarters{lc($company)}\',\'$DM_CompanySize{lc($company)}\',\'$DM_CompanyFounded{lc($company)}\',\'$DM_CompanySpecialties{lc($company)}\',\'$DM_CompanyProfileLink{lc($company)}\',$DM_CompanyFollowers{lc($company)},\'$DM_CompanyDescription{lc($company)}\',\'$company_link\',\'$con_city\','',\'$con_country\',\'$Contact_Region\','DataMiner')\,";
						if( $takencontacts >= $MaxContacts )
						{
							$coninsertquery =~s/\,\s*$//igs;
							$coninsertquery .= ';';
							&insert_query($coninsertquery);
							goto companyconcomplete;
						}
					}
				}
				elsif($single_person_cont=~m/(?:link_nprofile_view_3|link_nprofile_view_headless)\"\:\"([^>]*?)\"\,/is)
				{
					my $P_link = &urlcheck($1);
					push(@array_linkedin_member,$P_link);
					
				}
			}
			
			foreach my $P_link (@array_linkedin_member)
			{
				$third_connection_count++;
				my ($fullname,$fullname,$Search_Result_Status,$Search_Result_Status1,$industry);		
				my $c_content = &getcontent($P_link,$ping_count,$ua,$cookie);			
				open (CON,">c_content.html");
				print CON $c_content;
				close CON;
				my ($desig);
				if($c_content=~m/<p\s*class\=\"title\s*\"[^<]*?>\s*([\w\W]*?)\s*<\/p>/is)
				{
					$desig= &trim($1);
				}
				while($c_content=~m/<li[^>]*?>\s*<a[^>]*?href=\'([^>]*?)\'/igs)
				{
					my $People_link = &urlcheck($1);
					my $People_cont = &getcontent($People_link,$ping_count,$ua,$cookie);
					my $company_match_flag = 0;
					eval{
					$People_cont = Encode::decode("UTF-8", $People_cont);
					};			
					open (CON,">People_cont_desg.html");
					print CON $People_cont;
					close CON;
					$ping_count++;
					my $People_cont_lower = lc($People_cont);
					my $desig_lower       = lc($desig);
					
					if($People_cont=~m/<h\d+>\s*<a[^>]*?href\=\'([^>]*?)\'[^>]*?>[^>]*?(?:<\/a>)?\s*<\/h\d+>\s*<p[^>]*?>\Q$desig_lower\E/is)
					{
						my $person_link = &urlcheck($1);
						my $person_cont = &getcontent($person_link,$ping_count,$ua,$cookie);
						eval{
						$person_cont = Encode::decode("UTF-8", $person_cont);
						};
						
						open (PCON,">person_cont.html");
						print PCON $person_cont;
						close PCON;
						$ping_count++;
						
						my $location  = &trim($1) if($person_cont =~ m/name\=\'location[^<]*?>([^<]*?)\</is); # Pattern match to get Person Location.
						$location =~ s/area\s*\,/\,/igs;
						my ($con_city,$con_country,$con_state);
						if($location =~ m/([^<]*?)\,([^<]*?)\,([^>]*?)$/is)
						{
							$con_city = &trim($1);
							$con_state = &trim($2);
							$con_country = &trim($3);
						}
						if($location =~ m/([^<]*?)\,([^>]*?)$/is)
						{
							$con_city = &trim($1);
							$con_country = &trim($2);
						}
						else
						{
							$con_country = $location;
						}
						$con_city =~ s/\s+area\W*\s*$//igs;
						$con_country =~ s/\s+area\W*\s*$//igs;
						$con_state =~ s/\s+area\W*\s*$//igs;
						$fullname 	   = &trim($1) if($person_cont =~ m/<span\s*class\=\"full\-name[^>]*?>([^>]*?)</is); # Pattern match to get Person Full Name.		
						$industry  = &trim($1) if($person_cont =~ m/industry\s*\"\s*title[^>]*?>\s*([^>]*?)\s*</is); # Pattern match to get Industry.
						my ($firstname,$lastname);
						if($fullname =~ m/([^>]*?)\s+(\S+)$/is)
						{
							$firstname = $1;
							$lastname = $2;
							if($firstname =~ m/(\bvan de\b|\bvan\b|\bde\b|\bda\b|\bal\b|\bla\b|\bvon\b|\bdu\b|\ble\b|\bles\b)/is)
							{
								my $match_key = $1;
								$lastname = $match_key.' '.$lastname;
								$firstname =~ s/\b$match_key\s*//igs;
								
							}
						}
						while($person_cont =~ m/<h\d+>([\w\W]*?)<\/h\d+>\s*<h\d+>([\w\W]*?)<\/h\d+>\s*<\/header>\s*<span\s*class\=\"experience\-date\-locale">\s*<time>([\w\W]*?Present[\w\W]*?)(?:\s*<span[^<]*?locality\">\s*([^<]*?))?(?:<span|<\/span)>/igs)
						{
							my $Current_Desig   = &trim($1);
							my $Current_Company = &trim($2);
							$company_match_flag = &company_match($incompany,$Current_Company);
							if($company_match_flag == 0 )
							{
								$unmatched_count++;
								next;
							}
							$lastname =~ s/\,[^>]*?$//igs;
							my $concat_name = lc($firstname.$lastname);
							my @db_matched = grep { $_ eq $concat_name } @array_exist_dbname;
							my @exe_matched = grep { $Current_Desig =~ m/\b$_\b/i } @exculsion_list;
							if(@exe_matched)
							{
								$unmatched_count++;
								# next;
							}
							if(@db_matched)
							{
								$duplicate_count++;
								next;
							}
							unless(@db_matched)
							{
								push(@array_exist_dbname,$concat_name);
								if(uc($companyinfo) eq 'Y')
								{
									unless($domain_company{lc($Current_Company)})
									{
										$Current_Company =~ s/amp\;//igs;
										$Current_Company =~ s/\,//igs;
										my $jsoncompanyurl = 'https://www.linkedin.com/ta/company?query='.$Current_Company;
										my $jsoncompanycontent = &getcontent($jsoncompanyurl,$ping_count,$ua,$cookie);
										$jsoncompanycontent =~ s/amp\;//igs;
										$jsoncompanycontent =~ s/\,//igs;
										my $temp_Company = quotemeta($Current_Company);
										my $temp_industry = quotemeta($industry);
										
										if($jsoncompanycontent =~ m/\{\"displayName\"\:\"$temp_Company\"[^\}]+?\"url\"\:\"([^<]*?)\"/is)
										{
											my $linkedincompanyurl= $1;
											$DM_CompanyProfileLink{lc($Current_Company)} = $linkedincompanyurl;
											my $linkedincompanycontent = &getcontent($linkedincompanyurl,$ping_count,$ua,$cookie);
											open sr,">linkedincompanycontent.html";
											print sr $linkedincompanycontent;
											close sr;
											&Company_details_collect($linkedincompanycontent,$Current_Company);
											
										}
										else
										{
											$domain_company{lc($Current_Company)} = "-";
										}
									}
								}
								my $company_link =$domain_company{lc($Current_Company)};
								my $company_link_ori = $company_link;
								$company_link_ori =~s/\'/''/igs;
								$company_link_ori =~s/^\s*\-\s*$//igs;
								$domain_company_website{lc($Current_Company)} = &trim($company_link_ori);
								unless($company_link =~ m/^\s*\-\s*$/is)
								{
									my $u = URI::URL->new($company_link);
									$company_link = $u->host();
									$company_link =~ s/^\s*www\.//igs;
								}
								my $Contact_Region;
								if($con_country)
								{
									$Contact_Region = &region_details($con_country);
								}
								$company_link =~s/\'/''/igs;
								$company_link =~s/^\s*\-\s*$//igs;
								$lastname =~s/\'/''/igs;
								$firstname =~s/\'/''/igs;
								$Current_Desig =~s/\'/''/igs;
								$person_link =~s/\'/''/igs;
								$Current_Company =~s/\'/''/igs;
								$DM_CompanyType{lc($Current_Company)} =~s/\'/''/igs;
								$DM_CompanyHeadquarters{lc($Current_Company)} =~s/\'/''/igs;
								$DM_CompanySize{lc($Current_Company)} =~s/\'/''/igs;
								$DM_CompanyFounded{lc($Current_Company)} =~s/\'/''/igs;
								$DM_CompanySpecialties{lc($Current_Company)} =~s/\'/''/igs;
								$DM_CompanyProfileLink{lc($Current_Company)} =~s/\'/''/igs;
								$DM_CompanyFollowers{lc($Current_Company)} =~s/\'/''/igs;
								$DM_CompanyFollowers{lc($Current_Company)} =~s/\,//igs;
								$DM_CompanyDescription{lc($Current_Company)} =~s/\'/''/igs;
								$DM_CompanyFollowers{lc($Current_Company)} = 'NULL'  if($DM_CompanyFollowers{lc($Current_Company)} eq "");
								$con_city =~s/\'/''/igs;
								$con_state =~s/\'/''/igs;
								$con_country =~s/\'/''/igs;
								$Contact_Region =~s/\'/''/igs;
								$takencontacts++;
								
								$coninsertquery .= "(\'$firstname\',\'$lastname\',\'$Current_Desig\',\'$Current_Company\',\'$person_link\',now(),\'$agent_name\',\'$masterid\',now(),SOUNDEX(\'$firstname\'),SOUNDEX(\'$lastname\'),\'NEW\',now(),\'$agent_name\',1,'LS_STATUS','LS_STATUS',\'$industry\',\'$company_link_ori\',\'$DM_CompanyType{lc($Current_Company)}\',\'$DM_CompanyHeadquarters{lc($Current_Company)}\',\'$DM_CompanySize{lc($Current_Company)}\',\'$DM_CompanyFounded{lc($Current_Company)}\',\'$DM_CompanySpecialties{lc($Current_Company)}\',\'$DM_CompanyProfileLink{lc($Current_Company)}\',$DM_CompanyFollowers{lc($Current_Company)},\'$DM_CompanyDescription{lc($Current_Company)}\',\'$company_link\',\'$con_city\',\'$con_state\',\'$con_country\',\'$Contact_Region\','DataMiner')\,";
								if( $takencontacts >= $MaxContacts )
								{
									$coninsertquery =~ s/\,\s*$//igs;
									$coninsertquery .= ';';
									&insert_query($coninsertquery);
									goto companyconcomplete;
								}
							}
						}
						if($Search_Result_Status==0)
						{
							$Search_Result_Status=$Search_Result_Status1;
						}		
						goto Out;
					}
				}
				Out:
			}
		}
	
}
unless($coninsertquery =~ m/values\s*$/is)
{
	$coninsertquery =~ s/\,\s*$//igs;
	$coninsertquery .= ';';
	&insert_query($coninsertquery);
}
print "coninsertquery :: $coninsertquery \n";
companyconcomplete:
print "takencontacts :: $takencontacts  $MaxContacts";
my $mastercompany_tablename = $project_id.'_mastercompanies';

my $status_update_query = "update $mastercompany_tablename set SCRAPE_STATUS = 2, Scrape_Date = now(),$date_column=now() where master_id = $masterid";
print "status_update_query :: $status_update_query";#<STDIN>;
&update_query($status_update_query) if($masterid);
$third_connection_skipped_count = $third_connection_skipped_count - $third_connection_count;

my $log_update_query = "update c_scrapper_log set MATCHED=$takencontacts,THIRD_CONNECTION_PROCESSED=$third_connection_count,THIRD_CONNECTION_SKIPPED=$third_connection_skipped_count,PAGES_NAVIGATED=$pages_navigated,UNMATCHED=$unmatched_count,DUPLICATE=$duplicate_count,STATUS=\'Researched\' ,END_TIME = now() where id =$latest_log_insert_id " ;
&update_query($log_update_query) if($latest_log_insert_id);
open net,">$dot_net_communication";
print net "1";
close net;
($incompany,$masterid,$inloacation_temp) = &input_datails($select_query);
if($incompany ne "")
{
	goto nextrecord;
}


if($loginecontent =~ m/<a[^<]*?href\=\"([^<]*?)\"[^>]*?>\s*Sign\s*Out\s*</is)
{
	my $signout_url = $1;
	$signout_url =~ s/amp\;//igs;
	my $home_content = &getcontent($signout_url,$ping_count,$ua,$cookie);
	open sr,">signout.html";
	print sr $home_content;
	close sr;
}


sub urlcheck()
{
	my $url = shift;
	$url=~s/&amp;/&/igs;
	$url=~s/\s+/ /igs;
	$url=~s/\\u002d/-/igs;
	if($url!~/http/is)
	{
		$url='https://www.linkedin.com'.$url;
	}
	
	return $url;
}
sub trim_json()
{
	my $text = shift;	
	$text =~ s/\\*u003cB\\*u003e/ /igs;	
	$text =~ s/\\*u003c\/*B\\*u003e/ /igs;
	$text =~ s/u002d/-/igs;
	$text =~ s/\&\#x2F\;/\//igs;
	$text =~ s/\&quot\;/"/igs;	
	$text =~ s/\&\#39\;/,/igs;
	$text =~ s/\s+/ /igs;
	$text =~ s/^\s*|\s*$//igs;	
	return $text;
}

sub getcontent()
{
	my $url = shift;
	my $ping_count = shift;
	my $ua  = shift;
	my $cookie= shift;
	my $recount = 0;
	my $rand=int(rand(5))+5;	
	print "Please Wait for $rand sec....\n";
	sleep($rand);
	$ping_count++;
	print "url :: $url\n";
	get_again:
	my $req = HTTP::Request->new(GET=>$url); 
	$req->header("Accept"=>"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"); 
	$req->header("Content-Type"=>"application/x-www-form-urlencoded"); 
	my $res = $ua->request($req); 
	$cookie->extract_cookies($res); 
	$cookie->save; 
	$cookie->add_cookie_header($req); 
	my $code = $res->code(); 
	print $code,"\n"; 
	if($code =~ m/50/is) 
	{ 
		$recount++;
		if($recount<=3)
		{
			sleep(5);
			goto get_again;
		}
	}
	my $content = $res->content();
	if($content=~m/fmt_displayValue\"\:\"1st\s*Connections\"\,\"isSelected\"\:true\,\"/is)
	{
		print "\nLinkedIn Blocked\n";
		open(F,">$log_file");
		print F "LinkedIn Blocked\n";
		close F;
		exit;
	}
	return($content);
}
sub postcontent()
{
	my $mainurl = shift;
	my $post_content  = shift;
	my $homeurl  = shift;
	my $ping_count = shift;
	my $ua  = shift;
	my $cookie= shift;
	my $recount = 0;
	my $rand=int(rand(5))+5;	
	print "Please Wait for $rand sec....\n";
	$ping_count++;
	sleep($rand);
	post_again:
	my $req=HTTP::Request->new(POST=>"$mainurl");
	$req->header("Host"=>"www.linkedin.com");
	$req->header("Accept"=>"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
	$req->header("Accept-Language"=>"en-US,en;q=0.5");
	$req->header("Accept-Encoding"=>"gzip, deflate");
	$req->header("Content-Type"=>"application/x-www-form-urlencoded");
	$req->header("Referer"=>"$homeurl");	
	$req->content($post_content);
	my $res1=$ua->request($req);
	$cookie->extract_cookies($res1);
	$cookie->save;
	$cookie->add_cookie_header($req);
	my $code1=$res1->code;
	my ($loc,$page_content);
	if($code1 =~ m/30/is) 
	{ 
		$loc = $res1->header('location');
		print "loc :: $loc \n";
		$req = HTTP::Request->new(GET=>$loc); 
		$req->header("Accept"=>"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"); 
		$req->header("Content-Type"=>"application/x-www-form-urlencoded");
		$res1 = $ua->request($req); 
		$cookie->extract_cookies($res1); 
		$cookie->save; 
		$cookie->add_cookie_header($req); 
		$code1 = $res1->code(); 
	} 
	if($code1=~m/20/is)
	{		
		$page_content=$res1->content();
		# print "page_content :: $page_content \n";
		
	}
	elsif($code1 =~ m/^50/is)
	{
		$recount++;
		if($recount<=3)
		{
			sleep(5);
			goto post_again;
		}
	}
	if($page_content=~m/fmt_displayValue\"\:\"1st\s*Connections\"\,\"isSelected\"\:true\,\"/is)
	{
		print "\nLinkedIn Blocked\n";
		open(F,">$log_file");
		print F "LinkedIn Blocked\n";
		close F;
		exit;
	}
	return($page_content);
}


sub dbconnection()
{
	my $connect_check = 0;
	connect_again:
	
	my $dbh = DBI->connect("DBI:mysqlPP:database=MVC;host=$db_host","$db_user","$db_pass",{mysql_enable_utf8 => 1});
	# my $db_checking;
	# eval {$db_checking = $dbh->ping()};
	# print "db_checking :: $db_checking \n";#exit;
	# if ( $db_checking != 1) 
	# {
		# sleep(30);
		# exit if($connect_check >=1);
		# print "Waiting for DB connection\n";
		# $connect_check++;
		# goto connect_again;
	# }
	return $dbh;
}

sub columnname_retrive()
{
	my $project_id = shift;
	my $mastercompany_tablename = $project_id.'_mastercompanies';
	my $mastercontacts_tablename = $project_id.'_mastercontacts';
	my $query = "select * from c_scrapper_settings where projectid = \'$project_id\'";
	print "query :: $query\n";
	my $sth = $dbh->prepare($query);
	my ($insert_out_query,$select_query,$OutCon_LastName,$OutCon_FirstName,@exculsion_list,$MaxContacts);
	if($sth->execute())
	{
		while(my $dbhash_result = $sth->fetchrow_hashref)
		{
			my $in_company 			= $dbhash_result->{In_Company};
			   $inloacation			= $dbhash_result->{In_Location};
			   $in_jobtitle			= $dbhash_result->{In_JobTitle};
			my $default_location 	= $dbhash_result->{Default_Location};
			my $default_jobtitle 	= $dbhash_result->{Default_Jobtitle};
			my $primary_jobtitle 	= $dbhash_result->{Primary_Jobtitle};
			my $secondary_jobtitle	= $dbhash_result->{Secondary_Jobtitle};
			my $exclusion_jobtitle  = $dbhash_result->{Exclusion_Jobtitle};
			   $OutCon_FirstName    = $dbhash_result->{OutCon_FirstName};
			   $OutCon_LastName     = $dbhash_result->{OutCon_LastName};
			my $OutCon_JobTitle     = $dbhash_result->{OutCon_JobTitle};
			my $OutCon_ProfileLink  = $dbhash_result->{OutCon_ProfileLink};
			my $OutCon_Company      = $dbhash_result->{OutCon_Company};
			my $OutCon_Domain  		= $dbhash_result->{OutCon_Domain};
			$MaxContacts 			= $dbhash_result->{MaxContacts};
			$email_dupecheck_project= $dbhash_result->{Email_DupeCheck_Project};
			$emailfinder_service	= $dbhash_result->{EmailFinder_Service};
			$companyinfo			= $dbhash_result->{CompanyInfo};
			$thirdconnection_process= $dbhash_result->{ThirdConnection_Process};
			$insert_out_query = "insert into $mastercontacts_tablename (FIRST_NAME,LAST_NAME,JOB_TITLE,DM_CompanyName,CONTACT_LINK,CREATED_DATE,CREATED_BY,MASTER_ID,Scrape_Date,First_Name_Soundx,Last_Name_Soundx,NEW_OR_EXISTING,$flag\_UPDATED_DATE,$flag\_AGENT_NAME,scrape_status,TR_CONTACT_STATUS,WR_CONTACT_STATUS,Contact_Industry,DM_CompanyWebsite,DM_CompanyType,DM_CompanyHeadquarters,DM_CompanySize,DM_CompanyFounded,DM_CompanySpecialties,DM_CompanyProfileLink,DM_CompanyFollowers,DM_CompanyDescription,Contact_Domain,CONTACT_CITY,CONTACT_STATE,CONTACT_COUNTRY,Contact_Region,contact_source) values";
			$primary_jobtitle =~ s/\&/\%26/igs;
			$secondary_jobtitle =~ s/\&/\%26/igs;
			# $exclusion_jobtitle =~ s/\&/\%26/igs;
			# $in_company =~ s/\&/\%26/igs;
			# print "in_company :: $in_company\n";
			# $in_jobtitle ='';
			# $primary_jobtitle ='do|did|done';
			# $secondary_jobtitle ='come|came|coming';
			# $exclusion_jobtitle ='why|what|when';
			# $default_jobtitle = 'do|did|done';
			if($in_jobtitle ne "")
			{
				if($in_jobtitle =~ m/exclusion/is)
				{
					@exculsion_list   = split /\|/, $exclusion_jobtitle;
					
				}
				$in_jobtitle =~ s/primary\|*/(\"$primary_jobtitle\") AND /igs;
				$in_jobtitle =~ s/secondary\|*/(\"$secondary_jobtitle\") AND /igs;
				$in_jobtitle =~ s/exclusion\|*//igs;
				$in_jobtitle =~ s/\|/ OR /igs;
				$in_jobtitle =~ s/\[|\]//igs;
				$in_jobtitle =~ s/and\s*$//igs;
				
			}
			else
			{
				$in_jobtitle = $default_jobtitle;
				$in_jobtitle =~ s/\&/\%26/igs;
				$in_jobtitle =~ s/^s\*|\s*$//igs;
			}
			@array_in_jobtitle = ();
			if(length ($in_jobtitle) > 1700)
			{
				my $last_value;
				if($in_jobtitle =~ m/\W AND \W/is)
				{
					open(FH,">$log_file");
					print FH "Jobtitle :~More than 1700 character and also have the \"AND\" keyword \n";
					close FH;
					exit;
				}
				if($in_jobtitle =~ m/\" OR \"/is)
				{
					$in_jobtitle =~ s/^\s*\(|\)\s*$//igs;
					while($in_jobtitle =~ m/([\w\W]{1,1600})/igs)
					{
						my $value = $1;
						$value = '"'.$last_value.$value if($last_value ne "");
						my @array_job;
						if(length($value) >= 1600)
						{
							@array_job = split(/" OR "/,$value);
						}
						$last_value = $array_job[-1];
						my $quote_last_value = quotemeta($last_value);
						$value =~ s/$quote_last_value$//is;
						$value =~ s/\"\s+OR\s+\"$/"/is;
						push(@array_in_jobtitle,$value);
					}

				}
				else
				{
					$in_jobtitle =~ s/^\s*\(|\)\s*$//igs;
					while($in_jobtitle =~ m/([\w\W]{1,1600})/igs)
					{
						my $value = $1;
						$value = $last_value.$value if($last_value ne "");
						my @array_job;
						if(length($value) >= 1600)
						{
							@array_job = split(/ OR /,$value);
						}
						$last_value = $array_job[-1];
						my $quote_last_value = quotemeta($last_value);
						$value =~ s/$quote_last_value$//is;
						$value =~ s/\s+OR\s+$//is;
						push(@array_in_jobtitle,$value);
					}
				}
			}
			else
			{
				push(@array_in_jobtitle,$in_jobtitle)
			}
			print "in_jobtitle :: $in_jobtitle- \n";
			print "exculsion_list :: @exculsion_list \n";
			if($inloacation =~ m/\[/is)
			{
				$inloacation =~ s/\[|\]//igs;
				my @company_list = split /\|/, $inloacation;
				$company_search_url_part = '&locationType=Y&f_G=';
				foreach my $single_company (@company_list)
				{
					my $Country_Abbr = $hash_country_pick_list{lc($single_company)};
					if($Country_Abbr)
					{
						$multiple_company_flag = 1;
						$company_search_url_part = $company_search_url_part .$Country_Abbr. '%3A0,';
					}
				}
				$company_search_url_part =~ s/\,\s*$//igs;
			}
			if($multiple_company_flag == 1)
			{
				$select_query = "select $in_company,master_id from $mastercompany_tablename  WHERE SCRAPE_STATUS = 1 and FLAG = \'$flag\' AND $flag\_AGENTNAME = \'$agent_name\' LIMIT 1;";
			}
			else
			{
				$select_query = "select $in_company,master_id,$inloacation from $mastercompany_tablename  WHERE SCRAPE_STATUS = 1 and FLAG = \'$flag\' AND $flag\_AGENTNAME = \'$agent_name\' LIMIT 1;";
			}
			# print "select_query :: $select_query\n";#exit;
			
		}
			
	}
	else
	{
		my $db_error_str = $DBI::errstr;
		print "QUERY:: $query\n";
		open(ERR,">>$excep_folder_name");
		print ERR "$query \nquery get following error $db_error_str\n";
		close ERR;
		$dbh=&dbconnection();
		&insert_error_log($db_error_str,'columnname_retrive');
	}
	
	$sth->finish();
	# print "insert_out_query $insert_out_query \n select_query :: $select_query \n in_jobtitle :: $in_jobtitle \n OutCon_LastName :: $OutCon_LastName\n OutCon_FirstName :: $OutCon_FirstName";exit;
	return($insert_out_query,$select_query,$in_jobtitle,$OutCon_LastName,$OutCon_FirstName,$MaxContacts,\@exculsion_list);
}


sub internetcheck()
{
	use Net::Ping;
	my $p = Net::Ping->new();
	my $internet_ping_count = 0;
	while(1)
	{
		printf "Checking for internet\n";
		if ($p->ping("www.linkedin.com"))
		{
			printf "Internet connection is active!\n";
			last; #break out of while loop if connection found
		}
		else
		{
			$internet_ping_count++;
			if($internet_ping_count<=3)
			{
				printf "Internet connection not active! Sleeping..\n";
				sleep 15;
			}
			else
			{
				printf "No Internet connection...\n";
				open(FH,">$log_file");
				print FH "LoginStatus:~No Internet\n";
				close FH;
				exit;
			}
		}   
	}
}

sub trim()
{
	my $text = shift;
	# $text =decode_entities($text);
	$text =~ s/\\*u003cB\\*u003e/ /igs;	
	$text =~ s/\\*u003c\/*B\\*u003e/ /igs;
	$text =~ s/u002d/-/igs;
	$text =~ s/<[^>]*?>//igs;
	$text =~ s/\&amp\;/&/igs;
	$text =~ s/\&\#39\;/,/igs;
	$text =~ s/\&nbsp\;/ /igs;
	$text =~ s/\&#8211\;/-/igs;
	$text =~ s/\&\#x27\;/'/igs;
	$text =~ s/\&\#x2F\;/\//igs;
	$text =~ s/\s+/ /igs;
	$text =~ s/^\s*|\s*$//igs;
	return $text;
}

sub update_query()
{
	my $update_query = shift;
	my $sth_in = $dbh->prepare($update_query);
	if($sth_in->execute())
	{
		print "\nRecord Update Successfully\n";
		$sth_in->finish();
	}
	else
	{
		my $db_error_str = $DBI::errstr;
		open (IQ,">>$excep_folder_name");
		print IQ "$update_query \n query get following error $db_error_str\n";
		close IQ;
		sleep 10;
		print "Query Failed Reconnect\n";
		$dbh=&dbconnection();
		&insert_error_log($db_error_str,'update_query');
	}
}

sub insert_query()
{
	my $insert_query = shift;
	my $sth_in = $dbh->prepare($insert_query);
	print $insert_query;
	if($sth_in->execute())
	{
		print "\nRecords Inserted Successfully\n";
		$sth_in->finish();
	}
	else
	{
		my $db_error_str = $DBI::errstr;
		open (IQ,">>$excep_folder_name");
		print IQ "$insert_query \n query get following error $db_error_str\n";
		close IQ;
		sleep 10;
		print "Query Failed Reconnect\n";
		$dbh=&dbconnection();
		&insert_error_log($db_error_str,'update_query');
	}
}
sub duplicate_check()
{
	#my $self = shift;
	my $complete_contact_status = shift;
	my $project_id = shift;
	my $OutCon_FirstName = shift;
	my $OutCon_LastName = shift;
	my $masterid = shift;
	my $mastercontacts_tablename = $project_id.'_mastercontacts';
	my $query;
	if(uc($email_dupecheck_project) eq 'Y')
	{
		$complete_contact_status =~ s/^\s*AND\s*//igs;
	    $query = "SELECT CONTACT_ID_P, FIRST_NAME,LAST_NAME,1 STATUS  FROM $mastercontacts_tablename WHERE $complete_contact_status
				UNION 
				SELECT CONTACT_ID_P, FIRST_NAME,LAST_NAME,2 STATUS FROM c_RejectedContacts WHERE PROJECTID = \'$project_id\'";
	}
	else
	{
		$query = "SELECT CONTACT_ID_P, FIRST_NAME,LAST_NAME,1 STATUS  FROM $mastercontacts_tablename WHERE MASTER_ID =\'$masterid\' 				$complete_contact_status
					UNION 
					SELECT CONTACT_ID_P, FIRST_NAME,LAST_NAME,2 STATUS FROM c_RejectedContacts WHERE PROJECTID = \'$project_id\' AND MASTER_ID = \'$masterid\'";
	}
	my $sth = $dbh->prepare($query);
	my $check_flag=0;
	my (%hash_linkedin_name,$row_count);
	$row_count = 0;
	if($sth->execute())
	{
		while(my $dbhash_result = $sth->fetchrow_hashref)
		{
			my $contact_id	= $dbhash_result->{CONTACT_ID_P};
			# my $name		= lc($dbhash_result->{FIRST_NAME}.$dbhash_result->{LAST_NAME});
			my $first_name  = lc($dbhash_result->{FIRST_NAME});
			my $last_name  = lc($dbhash_result->{LAST_NAME});
			$last_name =~ s/\,[^<]*?$//igs; 
			my $STATUS		= $dbhash_result->{STATUS};
			my $name = lc($first_name.$last_name);
			$hash_linkedin_name{$contact_id} = $name;
			$row_count++ if($STATUS == 1);
			
		}
	}
	else
	{
		my $db_error_str = $DBI::errstr;
		open fh,">>$excep_folder_name";
		print fh "$query \n query get following error $db_error_str\n";
		close fh;
		sleep 10;
		print "Query Failed Reconnect\n";
		$dbh=&dbconnection();
		&insert_error_log($db_error_str,'duplicate_check');
	}
	return (\%hash_linkedin_name,$row_count);
}

sub Clean_Search_Term()
{
	#my $self = shift;
	my $Text=shift;
	$Text=~s/\s+(?:SERVICES|HOME|pvt|LTD|COUNCIL|UNIVERSITY|DESIGN|BANK|AGENCY|CONSULTING|UK|PLC|LLP|LIMITED|SOLUTIONS|Organisations|SCHOOL|CO|COLLEGE|CENTRE|PUBLISHING|ENGINEERING|ORGANIZATIONS|SERVICE|CONSULTANCY|SUPPLIES|PCT|HOTEL|OFFICE|COMPANY|Mills|LLC|private)\.?\s*$//igs;	
	$Text=~s/\s\s+/ /igs;
	$Text=~s/\W/ /igs;	
	return($Text);
}


sub input_datails()
{
	my $query = shift;
	print "query :: $query\n";
	my $sth = $dbh->prepare($query);
	my ($company_name,$company_country,$master_id);
	if($sth->execute())
	{
		if(my @record = $sth->fetchrow)
		{
			$company_name    = &trim($record[0]);
			$master_id = &trim($record[1]);
			$company_country = &trim($record[2]) if($multiple_company_flag == 0);
			$company_name =~ s/\&/\%26/igs;
			
		}
		else
		{
			# exit;
		}
		$sth->finish();
		
	}
	else
	{
		my $db_error_str = $DBI::errstr;
		open fh,">>$excep_folder_name";
		print fh "$query query get following error $db_error_str\n";
		close fh;
		# sleep ;
		print "Query Failed Reconnect\n";
		$dbh=&dbconnection();
		&insert_error_log($db_error_str,'input_datails');
	}
	# print "$company_name,$company_country,$master_id\n";exit;
	
	return ($company_name,$master_id,$company_country);
}

sub common_word_datails()
{
	my $query = 'select LEGAL_NAME from Company_Legal_Des';
	
	my $sth = $dbh->prepare($query);
	my @common_word_list;
	if($sth->execute())
	{
		while(my @record = $sth->fetchrow)
		{
			my $value    = &trim($record[0]);
			my $decomposed = NFKD( $value );
			$decomposed =~ s/\p{NonspacingMark}//g;
			$value = quotemeta($decomposed);
			push(@common_word_list,$value)
		}
		$sth->finish();
		
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
	
	return (\@common_word_list);
}

sub company_match()
{
	my $input_company = shift;
	my $linkedin_company = shift;
	my $company_match_status = 0;
	$input_company = &match_trim($input_company);
	$linkedin_company = &match_trim($linkedin_company);	
	if($input_company eq $linkedin_company)
	{
		$company_match_status++;
		return($company_match_status);
	}
	else
	{
		my @remain_input_company = split(' ',$input_company);
		my @remain_linkedin_company = split(' ',$linkedin_company);
		foreach my $input_comp_word (@remain_input_company)
		{
			$input_comp_word =~ s/~~/ /igs;
			$input_comp_word =~ s/\s+/ /igs;
			$input_comp_word =~ s/^\s+|\s+$//igs;
			next if(length($input_comp_word)<=1);
			foreach my $input_link_comp_word (@remain_linkedin_company)
			{
				$input_link_comp_word =~ s/~~/ /igs;
				$input_link_comp_word =~ s/\s+/ /igs;
				$input_link_comp_word =~ s/^\s+|\s+$//igs;
				next if(length($input_link_comp_word)<=1);
				if($input_comp_word eq $input_link_comp_word)
				{
					$company_match_status++;
					return($company_match_status);
					# last;
				}
			}
		}
	}
	return($company_match_status);
	
}

sub match_trim()
{
	my $text = lc(shift);
	$text =~ s/\W/ /igs;
	my $decomposed = NFKD( $text );
	$decomposed =~ s/\p{NonspacingMark}//g;
	$text = $decomposed;
	grep { $text =~ s/\b$_\b//is } @common_word_list;
	while($text =~ m/\s(\w\s){2,}/igs)
	{
		my $single_match_value = $&;
		print "single_match_value :; $single_match_value";
		my $single_match_value_space = $single_match_value;
		$single_match_value_space =~ s/\s/~~/igs;
		$text =~ s/$single_match_value/ $single_match_value_space-/is;
		$text =~ s/\-(\w)$/$1/igs;
		$text =~ s/\-/ /igs;
		$text =~ s/\s+/ /igs;
	}
	# print "text :: $text \n";
	$text =~ s/\s+/ /igs;
	$text =~ s/^\s+|\s+$//igs;
	return($text);
}

sub insert_log()
{
	my $project_id = shift;
	my $masterid = shift;
	my $search_url = shift;
	my $username = shift;
	my $search_result = shift;
	my $agent_name = shift;
	my $flag = shift;
	my $SYSTEM_NAME = shift;
	my $SCRAPPER_VERSION = shift;
	my $log_session_id = shift;
	$search_url =~ s/\'/''/igs;
	$SYSTEM_NAME =~ s/\'/''/igs;
	my $query = "insert into c_scrapper_log (PROJECTID,COMPANYID,LOGIN,SEARCH_URL,FOUND,AGENTNAME,RESEARCH_TYPE,START_TIME,SYSTEM_NAME,SCRAPPER_VERSION,sessionid) values(\'$project_id\',$masterid,\'$username\',\'$search_url\',$search_result,\'$agent_name\',\'$flag\',now(),\'$SYSTEM_NAME\',\'$SCRAPPER_VERSION\',\'$log_session_id\')";
	my $sth_in = $dbh->prepare($query);
	if($sth_in->execute())
	{
		my $idx = $dbh->{mysql_insertid};
		$sth_in->finish();
		return($idx);
	}
	else
	{
		my $db_error_str = $DBI::errstr;
		
		open (IQ,">>$excep_folder_name");
		print IQ "$query \nquery get following error $db_error_str\n";
		close IQ;
		# sleep 10;
		print "Query Failed Reconnect\n";
		$dbh=&dbconnection();
		&insert_error_log($db_error_str,'insert_log');
	}
}

sub insert_error_log()
{
	my $err_message = shift;
	my $Target_Site = shift;
	$err_message =~ s/\'/''/igs;
	
	my $query = "insert into error_log (PROJECTID,UserType,Agent,Error_Date,Message,Handled,Machine,SoftwareVersion,Target_Site) values(\'$project_id\',\'$flag\',\'$agent_name\',now(),\'$err_message\','Yes',\'$host_name\',\'$SCRAPPER_VERSION\',\'$Target_Site\')";
	my $sth_in = $dbh->prepare($query);
	if($sth_in->execute())
	{
		my $idx = $dbh->{mysql_insertid};
		$sth_in->finish();
		return($idx);
	}
	else
	{
		my $db_error_str = $DBI::errstr;
		open (IQ,">>$excep_folder_name");
		print IQ "$query \nquery get following error $db_error_str\n";
		close IQ;
		# sleep 10;
		print "Query Failed Reconnect\n";
		$dbh=&dbconnection();
	}
}

# &insert_log($project_id,$masterid,$username,$search_url,$search_result,$agent_name,$flag);

sub check_cm_status()
{
	my $pi = Win32::Process::Info->new ();
	$pi->Set (elapsed_in_seconds => 0);    # In clunks, not seconds.
	my  @info = $pi->GetProcInfo ();   # Get the max
	@info = grep {
	 defined $_->{Name} &&
	 $_->{Name} =~ m/Campaign Manager/
	} $pi->GetProcInfo ();        # All processes with 'perl' in name.
	unless(@info)
	{
		open(FH,">$log_file");
		print FH "LoginStatus:~CM Inactive \n";
		close FH;
		exit;
	}
}


sub region_details()
{
	my $country   = shift;
	# print "query :: $query\n";
	my $query  = 'select region from country where countryname = \''.$country.'\'';
	my $sth = $dbh->prepare($query);
	my ($region);
	if($sth->execute())
	{
		if(my @record = $sth->fetchrow)
		{
			$region    = &trim($record[0]);
		}
		else
		{
			# exit;
		}
		$sth->finish();
		
	}
	else
	{
		my $db_error_str = $DBI::errstr;
		open fh,">>$excep_folder_name";
		print fh "$query \nquery get following error $db_error_str\n";
		close fh;
		# sleep ;
		print "Query Failed Reconnect\n";
		$dbh=&dbconnection();
		&insert_error_log($db_error_str,'region_details');
	}
	# print "$company_name,$company_country,$master_id\n";exit;
	
	return ($region);
}

sub get_session_id()
{
	my ($Second, $Minute, $Hour, $Day, $Month, $Year, $WeekDay, $DayOfYear, $IsDST) = localtime(time);
	$Year +=1900;
	$Month++;
	$Day = '0'.$Day if(length($Day) == 1);
	$Month = '0'.$Month if(length($Month) == 1);
	$Hour = '0'.$Hour if(length($Hour) == 1);
	$Second = '0'.$Hour if(length($Second) == 1);
	$Minute = '0'.$Hour if(length($Minute) == 1);
	my($addr) = inet_ntoa( (gethostbyname(hostname()))[7] );
	my $seesionid = $addr.$Day.$Month.$Year.$Hour.$Second.$Minute;
	$seesionid =~ s/\D//igs;
	return($seesionid);
}

sub picklist_details()
{
	my $query = "select PicklistCategory,PicklistField,PicklistValue from c_picklists where Picklistcategory in ('MailConfig','Path')";
	# print "query :: $query\n";
	my $sth = $dbh->prepare($query);
	my ($PicklistCategory,$PicklistField,$PicklistValue);
	if($sth->execute())
	{
		while(my @record = $sth->fetchrow)
		{
			$PicklistCategory    = &trim($record[0]);
			$PicklistField = &trim($record[1]);
			$PicklistValue = &trim($record[2]);
			my $key = $PicklistCategory.'_'.$PicklistField;
			$picklist_detail{$key} = $PicklistValue;
			
		}
		$sth->finish();
		
	}
	else
	{
		my $db_error_str = $DBI::errstr;
		open fh,">>$excep_folder_name";
		print fh "$query query get following error $db_error_str\n";
		close fh;
		# sleep ;
		print "Query Failed Reconnect\n";
		$dbh=&dbconnection();
		&insert_error_log($db_error_str,'picklist_details');
	}
	# print "$company_name,$company_country,$master_id\n";exit;
	
	# return ($company_name,$company_country,$master_id);
}

sub Company_details_collect()
{
	my $linkedincompanycontent = shift;
	my $Current_Company = shift;
	if($linkedincompanycontent =~ m/<h\d+[^>]*?>\s*Website\s*<\/h\d+>([\w\W]*?)\s*<\/p>/is)
	{
		my $companylink = &trim($1);
		$domain_company{lc($Current_Company)} = $companylink;
		$domain_company{lc($Current_Company)} =~ s/^\s*http:\/\/\s*$/-/igs;
	}
	else
	{
		$domain_company{lc($Current_Company)} = "-";
	}
	if($linkedincompanycontent =~ m/<h\d+[^>]*?>\s*Specialties\s*<\/h\d+>([\w\W]*?)\s*<\/p>/is)
	{
		my $value = &trim($1);
		$DM_CompanySpecialties{lc($Current_Company)} = $value;
	}
	if($linkedincompanycontent =~ m/<h\d+[^>]*?>\s*Headquarters\s*<\/h\d+>\s*<p[^>]*?>\s*([\w\W]*?)\s*<\/p>/is)
	{
		my $value_temp = $1;
		my $value;
		while($value_temp =~ m/<span[^<]*?class\=\"street\-address\"[^>]*?>([\w\W]*?)<\/span>/igs)
		{
			$value = $value.$1.",";
		}
		$value =~ s/\,\s*$/|/igs;
		if($value eq "")
		{
			$value = $value."|";
		}
		if($value_temp =~ m/<span[^<]*?class\=\"locality\"[^>]*?>([\w\W]*?)<\/span>/is)
		{
			$value = $value.$1."|";
		}
		else
		{
			$value = $value."|";
		}
		if($value_temp =~ m/<abbr[^<]*?class\=\"region\"[^>]*?>([\w\W]*?)<\/abbr>/is)
		{
			$value = $value.$1."|";
		}
		else
		{
			$value = $value."|";
		}
		if($value_temp =~ m/<span[^<]*?class\=\"postal\-code\"[^>]*?>([\w\W]*?)<\/span>/is)
		{
			$value = $value.$1."|";
		}
		else
		{
			$value = $value."|";
		}
		if($value_temp =~ m/<span[^<]*?class\=\"country\-name\"[^>]*?>([\w\W]*?)<\/span>/is)
		{
			$value = $value.$1;
		}
		$value =~ s/\|\s*|\,\|\s*/|/igs;
		$DM_CompanyHeadquarters{lc($Current_Company)} = $value;
	}
	if($linkedincompanycontent =~ m/<h\d+[^>]*?>\s*Company\s*Size\s*<\/h\d+>([\w\W]*?)\s*<\/p>/is)
	{
		my $value = &trim($1);
		$value =~ s/\s*employees*\s*//igs;
		$value =~ s/\,//igs;
		$DM_CompanySize{lc($value)} = $value;
	}
	if($linkedincompanycontent =~ m/<h\d+[^>]*?>\s*Type\s*<\/h\d+>([\w\W]*?)\s*<\/p>/is)
	{
		my $value = &trim($1);
		$DM_CompanyType{lc($Current_Company)} = $value;
	}
	if($linkedincompanycontent =~ m/<h\d+[^>]*?>\s*Founded\s*<\/h\d+>([\w\W]*?)\s*<\/p>/is)
	{
		my $value = &trim($1);
		$DM_CompanyFounded{lc($Current_Company)} = $value;
	}
	if($linkedincompanycontent =~ m/<\/h\d+>\s*<p[^>]*?>\s*([^<]*?)followers\s*<\/p>/is)
	{
		my $value = &trim($1);
		$value =~ s/\D//igs;
		$DM_CompanyFollowers{lc($Current_Company)} = $value;
	}
	elsif($linkedincompanycontent =~ m/>\s*([^<]*?)<\/span>\s*<span[^>]*?>\s*followers<\/span>/is)
	{
		my $value = &trim($1);
		$value =~ s/\D//igs;
		$DM_CompanyFollowers{lc($Current_Company)} = $value;
		
	}
	
	if($linkedincompanycontent =~ m/<meta[^<]*?property\=\"og\:description\"\s*content\=\"([^<]*?)\"/is)
	{
		my $value = &trim($1);
		$DM_CompanyDescription{lc($Current_Company)} = $value;
	}
}