var server = require("./server");
var router = require("./router");
var requestHandlers = require("./requestHandlers");

var handles = {};

handles["/"] = requestHandlers.start;
handles["/start"] = requestHandlers.start;
handles["/upload"] = requestHandlers.upload;

server.start(router.route , handles);