var parsley = require('parsley');
var net = require('net');
var fs = require('fs');

var config = require('./config.json');
port = config['port'];
logfile = config['log'];

function simpleLog(string) {
    console.log(string);
    fs.appendFile(logfile, string);
}

net.createServer(function (stream) {
    parsley(stream, function (req) {
        var head = [];
        var body = [];

        req.on('rawHead', function (buf) {
            head.push(buf);
        });

        req.on('rawBody', function (buf) {
            body.push(buf);
        });

        req.on('end', function () {
            //log all raw request/responses
            simpleLog(head.map(String).join(''));
            simpleLog(body.map(String).join(''));

            //return success for all requests
            stream.write('HTTP/1.1 200 OK\r\n');
            stream.write('Content-Type: text/plain\r\n');
            stream.write('\r\n');
            stream.end();
        });
    });
}).listen(port);

console.log('Server is listening on port ' + port + "\n");
