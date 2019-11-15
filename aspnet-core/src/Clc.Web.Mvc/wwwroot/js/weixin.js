(function() {
    var checkSubmitFlg = false; 

    function onSubmit(){ 
        if(checkSubmitFlg == true) return false; //当表单被提交过一次后checkSubmitFlg将变为true,根据判断将无法进行提交。
        checkSubmitFlg = true; 
        return true; 
    }
});