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
        var User_Name = $('#Name').val();
        var Username = $('#Username').val();
        var Password = $('#Password').val();
        var CPassword = $('#CPassword').val();
        var User_Email = $('#Email').val();
        var User_Phone = $('#Phone').val();

        if (ValidateEmail(User_Email) ) {
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

    $('#btnAddUser').click(function () {
        if (Validate() == true) {
            var User_ID = $('#UserID').val();
            var User_Name = $('#Name').val();
            var User_Address = $('#Address').val();
            var User_Designation = $('#Designation').val();
             var UserType = $('#UserType option:selected').val();
            var User_Email = $('#Email').val();
            var User_Phone = $('#Phone').val();
            var Username = $('#Username').val();
            var Password = $('#Password').val();

            var data = new FormData();
            data.append("User_ID", User_ID);
            data.append("User_Name", User_Name);
            data.append("User_Address", User_Address);
            data.append("User_Designation", User_Designation);
        data.append("User_Type", UserType);
            data.append("User_Email", User_Email);
            data.append("User_Phone", User_Phone);
            data.append("User_Username", Username);
            data.append("User_Password", Password);

            $.ajax({
                type: "POST",
                url: "/Master/SaveUser",
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
                    //$(window).scrollTop(0);
                    //$(".messagebox").append('<div class="well bg-primary msg"><strong> Error!</strong> Failed To Save.</div>');
                    //$(".msg").delay(4000).fadeOut(800);  
                }
            }
            function OnErrorSaveCall() {
                alert("error");
                //$(window).scrollTop(0);
                //$(".messagebox").append('<div class="well bg-primary msg"><strong> Error!</strong> Failed To Save.</div>');
                //$(".msg").delay(4000).fadeOut(800);
            }
        }
    });

    $(".btnDeleteUser").click(function () {      
        if (confirm("Are You Sure You Want To Delete?")) {
            var row = $(this).closest('tr');
            var userid = $(row).find('td:eq(3)').find('input').val();
            $.ajax({
                type: "POST",
                url: "/Master/DeleteUser",
                data: "{'User_ID':'" + userid + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                //beforeSend: function () { $("#loader").css("display", "block"); },
                //complete: function () { $("#loader").css("display", "none"); },
                success: OnSuccessSaveCall,
                error: OnErrorSaveCall
            });
            function OnSuccessSaveCall(data) {
                if (data > 0) {
                    $("#" + userid).fadeOut("slow").remove(); alert("success");
                    //$(window).scrollTop(0);
                    //$(".messagebox").append('<div class="well bg-success msg"><strong> Success!</strong> Delete Successful.</div>');
                    //$(".msg").delay(4000).fadeOut(800);
                }
                else {
                    alert("error");
                    //$(window).scrollTop(0);
                    //$(".messagebox").append('<div class="well bg-primary msg"><strong> Error!</strong> Failed To Delete.</div>');
                    //$(".msg").delay(4000).fadeOut(800);
                }
            }
            function OnErrorSaveCall() {
                alert("error");
                //$(window).scrollTop(0);
                //$(".messagebox").append('<div class="well bg-primary msg"><strong> Error!</strong> Failed To Delete.</div>');
                //$(".msg").delay(4000).fadeOut(800);
            }
        }
    });
});