//app.js
http = require('http');
var app = require('http').createServer(handler)
, io = require('socket.io').listen(app)
, fs = require('fs')

app.listen(8080);

function handler (req, res) {
fs.readFile(__dirname + '/index.html',
function (err, data) {
  if (err) {
    res.writeHead(500);
  return res.end('Error loading index.html');
 }

res.writeHead(200);
res.end(data);
});
}

io.sockets.on('connection', function (socket) {
getStatusUpdate(function(result) {
  var returedData = result
  console.log ('2: ', returedData);
    socket.emit('status', { args: returedData });
});
});

function getStatusUpdate(callback) {
var server = http.createServer(function (request, response) {

var body = '';

request.addListener('data', function(chunk){
  console.log('got a chunk');
  body += chunk;
});


request.addListener('error', function(error){
  console.error('got a error', error);
  next(err);
});

request.addListener('end', function(chunk){
  console.log('ended');
  if (chunk) {
    body += chunk;
  }

  console.log('1:', body);
  response.write( body);
  response.end();

  callback(body);

});

  });

  port = 3351;
  host = '127.0.0.1';
  server.on('listening',function(){
  console.log('ok, server is running');
  });
  server.listen(port, host);
  console.log('Listening at http://' + host + ':' + port);
  };