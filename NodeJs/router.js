function route(handles , pathname ) {
  console.log("About to route a request for " + pathname);
  var handle = handles[pathname];
  if(typeof(handle) == 'function'){
    handle();
  } else {
    console.log("No request handler found for "+ pathname);
  }
}

exports.route = route;