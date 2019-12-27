var checkSubmitFlg = false; 

function checkSubmit() { 
    if(checkSubmitFlg == true) return false; //当表单被提交过一次后checkSubmitFlg将变为true,根据判断将无法进行提交。
    checkSubmitFlg = true; 
    return true; 
}

function myajax(url, params, callback){
    //1.创建ajax对象
    var xmlHttp = new XMLHttpRequest();
    //2.绑定监听函数
    xmlHttp.onreadystatechange = function(){
        //判断数据是否正常返回
        if(xmlHttp.readyState==4&&xmlHttp.status==200){
            //6.接收数据
            var res = xmlHttp.responseText;
            callback(res);          //document.getElementById("span1").innerHTML = res;
        }
    }

    //3.绑定处理请求的地址,true为异步，false为同步
    //GET方式提交把参数加在地址后面?key1:value&key2:value
    xmlHttp.open("POST", url, true);
    //4.POST提交设置的协议头（GET方式省略）
    xmlHttp.setRequestHeader("Content-type","application/x-www-form-urlencoded");
    //POST提交将参数，如果是GET提交send不用提交参数
    //5.发送请求
    xmlHttp.send(params);
}
