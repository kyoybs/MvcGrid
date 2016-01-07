  var upload1file = function(fileURL, successCall, failCall){
        var options = 'aaaa';
        options.fileKey="file";
        options.fileName=fileURL.substr(fileURL.lastIndexOf('/')+1);

        options.params = {'passport':'abc', 'retType':'json'};
 
        var ft = {};
       
        ft.upload = function(a,b,c,d,e){ 
            var args = []; 
            c.apply(this, args);
            };
        ft.upload(fileURL, '', 
            function(){ 
                alert(options);
                //options;
            }, function(er){
            }, 
            options
        );
    }
    
upload1file('url',null,null);
 