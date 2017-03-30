simple-http-listener
====================

A basic http server for node.js that logs all raw incoming requests. Currently the server returns 200 OK to all requests.

Uses [parsley](https://github.com/substack/node-parsley) to obtain the raw HTTP requests

Usage
-----------
Get a copy of simple-http-listener:

    git clone git://github.com/jiangk/simple-http-listener.git

Run the server:

    cd simple-http-listener
    node simple-http-listener

Configuration
-----------
Modify config.json to change configuration:

*   log file (default log.txt)
*   listening port (default 8080)

Motivation
-----------
During a CTF competition my team discovered the ability to make a remote service send out some unknown but predefined HTTP requests to a server of our choice. We wanted to find out what exactly the request looked like. Access logs for most HTTP servers don't provide the full request, so I wrote this simple server. 

