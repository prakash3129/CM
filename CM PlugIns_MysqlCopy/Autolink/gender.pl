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
use JSON qw( decode_json );



# my @common_word_list = @$common_word_ref;
# my @exculsion_list = @$exculsion_list_ref;
my $ua=LWP::UserAgent->new(ssl_opts => { verify_hostname => 0 }, show_progress=>1);
$ua->agent("Mozilla/5.0 (Windows NT 5.1; rv:18.0) Gecko/20100101 Firefox/18.0");
$ua->timeout(90);
my $cookie=HTTP::Cookies->new(file=>$0."_cookie.txt", autosave=>1);
$ua->cookie_jar($cookie);










my $file = 'd:\ss.csv';
open(my $data, '+<', $file) or die "could not open '$file' $!\n";
# open(fh, "+<out.txt")
while (my $line = <$data>)
{

   open op,">>Res.csv";
   chomp $line;
   
   my @fields = split "," , $line;
   print $fields[0];
   #print $data $fields[1];
   
   
   my $search_url = 'https://api.genderize.io/?name='.$fields[0];
my $search_cont = &getcontent($search_url,$ua,$cookie);

print $search_cont;

my $decoded = decode_json($search_cont);
#print "city = " . $decoded->{'gender'}. "\n";
   
   
   print op "$fields[0],$decoded->{'gender'}\n";
   close op;
   # print $data $fields[1];
   
   #push @fields, $fields;   
   #print $data shift(@fields);
}
close $data;


# my $search_url = 'https://api.genderize.io/?name[0]=assi';
# my $search_cont = &getcontent($search_url,$ua,$cookie);

# print $search_cont;

# my $decoded = decode_json($search_cont);

# print "Name = " . $decoded->{'name'}. "\n";
# print "City = " . $decoded->{'gender'}. "\n";


sub trim_json()
{
	my $text = shift;	
	$text =~ s/\\*u003cB\\*u003e/ /igs;	
	$text =~ s/\\*u003c\/*B\\*u003e/ /igs;
	$text =~ s/u002d/-/igs;
	$text =~ s/\&\#x2F\;/\//igs;
	$text =~ s/\&quot\;/"/igs;	
	$text =~ s/\s+/ /igs;
	$text =~ s/^\s*|\s*$//igs;	
	return $text;
}

sub getcontent()
{
	my $url = shift;	
	my $ua  = shift;
	my $cookie= shift;
	my $recount = 0;
	my $rand=int(rand(3));	
	print "Please Wait for $rand sec....\n";
	sleep($rand);	
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
	
	return($content);
}