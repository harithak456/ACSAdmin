$(document).ready(function () {

    function ValidateEmail(inputText) {
        var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
        if (!inputText.match(mailformat)) {
            return (true)
        }
        else {
            return (false)
        }
    }

    function phonenumber(inputtxt) {
        var phoneno = /^\d{10}$/;
        if (!inputtxt.match(phoneno)) {
            return (true)
        }
        else { return false; }
    }

    function Validate() {
        var flag = true;
        $(".inputValid").html("");
        var Email = $('#REmail').val();
        var Password = $('#RPassword').val();

        if (ValidateEmail(User_Email)) {
            $("#validMail").html("Enter a Valid Email Id.");
            flag = false;
        }
        if (phonenumber(User_Phone) && User_Phone.trim() != "") {
            $("#validPhone").html("Enter a Valid Mobile No.");
            flag = false;
        }

        if (User_Name == null || User_Name.trim() == "") {
            $("#validName").html("This field is required.");
            flag = false;
        }
        if (Username == null || Username.trim() == "") {
            $("#validUsername").html("This field is required.");
            flag = false;
        }
        if (Password == null || Password.trim() == "") {
            $("#validPassword").html("This field is required.");
            flag = false;
        }
        if (CPassword == null || CPassword.trim() == "") {
            $("#validCPassword").html("This field is required.");
            flag = false;
        }

        if (Password != "" && CPassword != "" && Password != CPassword) {
            $("#validCPassword").html("Password Mismatch.");
            flag = false;
        }
        return flag;
    }

    $('#btnRegister').click(function () {
        //if (Validate() == true) {        
            var Email = $('#REmail').val();
            var Password = $('#RPassword').val();

            var data = new FormData();         
            data.append("Guest_Username", Email);
            data.append("Guest_Password", Password);

            $.ajax({
                type: "POST",
                url: "/Login/RegisterGuest",
                data: data,
                contentType: false,
                processData: false,
                dataType: "json",
                //beforeSend: function () { $("#loader").css("display", "block"); },
                //complete: function () { $("#loader").css("display", "none"); },
                success: OnSuccessSaveCall,
                error: OnErrorSaveCall
            });

            function OnSuccessSaveCall(data) {
                if (data > "0") {
                    alert("success");
                    //$(window).scrollTop(0);
                    //$(".messagebox").append('<div class="well bg-success msg"><strong> Success!</strong> Save Successful.</div>');
                    //$(".msg").delay(8000).fadeOut(800);
                    //setTimeout(
                    //    function () {
                    //        location.href = "/Master/Users";
                    //    }, 800);
                    location.href = "/Master/Users";
                }
                else {
                    alert("error");
                }
            }
            function OnErrorSaveCall() {
                alert("error");
               
            }
        //}
    });

    $("#btnLogin").click(function () {
        var Username = $("#Username").val();
        var Password = $("#Password").val();
        $.ajax({
            type: "POST",
            url: "/Login/CreateLogin",
            data: "{'Username':'" + Username + "','Password':'" + Password + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: OnSuccessCall,
            error: OnErrorCall
        });
        function OnSuccessCall(data) {
            console.log(data);
            if (data == "1") {
                console.log(data);
                location.href = "/Home/Index";
            } else if (data == "-1") {
                alert("Please Check Your Username and Password");
            } else if (data == "0") {
                alert("Please Fill Details");
            }
        }
        function OnErrorCall() {
            alert("error");
        }
    });   
});