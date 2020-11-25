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
        var phone = /^(?:\+971|00971|0)(?:2|3|4|6|7|9|50|51|52|55|56)[0-9]{7}$/;

        if (!inputtxt.match(phone)) {
            return (true);
        }
        else {
            return false;
        }

    }

    function Validate() {
        var flag = true;
        $(".inputValid").html("");

        var FName = $('#FName').val();
        var LName = $('#LName').val();
        var Email = $('#REmail').val();
        var Password = $('#RPassword').val();
        var Phone = $('#Phone').val();

        if (ValidateEmail(Email)) {
            $("#validEmail").html("Enter a Valid Email Id.");
            flag = false;
        }
        //if (phonenumber(Phone) ) {
        //    $("#validPhone").html("Enter a Valid Mobile No.");
        //    flag = false;
        //}
        if (Phone == null || Phone.trim() == "") {
            $("#validPhone").html("Enter a Valid Mobile No.");
            flag = false;
        }
        if (FName == null || FName.trim() == "") {
            $("#validFName").html("This field is required.");
            flag = false;
        }
        if (LName == null || LName.trim() == "") {
            $("#validLName").html("This field is required.");
            flag = false;
        }     
        if (Password == null || Password.trim() == "") {
            $("#validPassword").html("This field is required.");
            flag = false;
        }       
        return flag;
    }

    $('#btnRegister').click(function () {
      
        if (Validate() == true) {
            var FName = $('#FName').val();
            var LName = $('#LName').val();
            var Email = $('#REmail').val();
            var Password = $('#RPassword').val();
            var Phone = $('#Phone').val();

            var data = new FormData();
            data.append("Guest_FirstName", FName);
            data.append("Guest_LastName", LName);
            data.append("Guest_Username", Email);
            data.append("Guest_Password", Password);
            data.append("Guest_Phone", Phone);

            $.ajax({
                type: "POST",
                url: "/Login/RegisterGuest",
                data: data,
                contentType: false,
                processData: false,
                dataType: "json",
                beforeSend: function () { $("#loader").css("display", "block"); },
                complete: function () { $("#loader").css("display", "none"); },
                success: OnSuccessSaveCall,
                error: OnErrorSaveCall
            });

            function OnSuccessSaveCall(data) {
                if (data > 0) {
                    $('#spanMsg').html("You have successfully created an account. Kindly verify your email address in the link provided to your mail");
                    $(window).scrollTop(0);
                    $('#FName').val('');
                    $('#LName').val('');
                    $('#REmail').val('');
                    $('#RPassword').val('');
                    $('#Phone').val('');
                }
                else if (data = -1) {
                    $('#spanMsg').html("This email is already registered !");
                    $(window).scrollTop(0);
                }
                else {
                    $('#spanMsg').html("Registration Failed !");
                    $(window).scrollTop(0);
                }
            }
            function OnErrorSaveCall() {
                $('#spanMsg').html("Registration Failed !");
                $(window).scrollTop(0);
            }
        }
        else {
           
        }
    });

    function Validation() {
        var flag = true;
        $(".inputValid").html("");

        var Username = $("#Username").val();
        var Password = $("#Password").val();

        if (ValidateEmail(Username)) {
            $("#validUsername").html("Enter a Valid Username/Email.");
            flag = false;
        }
       
        if (Password == null || Password.trim() == "") {
            $("#validPass").html("This field is required.");
            flag = false;
        }     
        return flag;
    }

    $("#btnLogin").click(function () {
        if (Validation() == true) {
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
                if (data == "1") {
                    location.href = "/Home/Index";
                } else if (data == "-1") {
                    $('#spanMsg').html("Please Check Your Username and Password");
                    $(window).scrollTop(0);
                } else if (data == "0") {
                    $('#spanMsg').html("Please Check Your Username and Password");
                    $(window).scrollTop(0);
                }
            }
            function OnErrorCall() {
                $('#spanMsg').html("Failed to login");
                $(window).scrollTop(0);
            }
        }
    });   
});