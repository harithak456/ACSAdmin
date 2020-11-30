$(document).ready(function () {

    function Validate() {
        var flag = true;
        $(".inputValid").html("");
        var Name = $('#OrgName').val();
        var Address = $('#Address').val();
        var Phone = $('#Phone').val();
        var Email = $("#Email").val();
        var ContactPerson = $('#ContactPerson').val();
        var Website = $('#Website').val();

        if (Name == null || Name.trim() == "") {
            $("#validName").html("This field is required.");
            flag = false;
        }
        if (Address == null || Address.trim() == "") {
            $("#validAddress").html("This field is required.");
            flag = false;
        }
        if (ValidateEmail(Email)) {
            $("#validMail").html("Enter a Valid Email Id.");
            flag = false;
        }
        //if (phonenumber(Phone)) {
        //    $("#validMobile").html("Enter a Valid Mobile No.");
        //    flag = false;
        //}
        if (Phone == null || Phone.trim() == "") {
            $("#validMobile").html("Enter a Valid Mobile No.");
            flag = false;
        }
        if (IsWebsite(Website)) {
            $("#validWebsite").html("Enter a Valid Website.");
            flag = false;
        }
        if (ContactPerson == null || ContactPerson.trim() == "") {
            $("#validContactPerson").html("This field is required.");
            flag = false;
        }
        return flag;
    }

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
        var land = /^\d{10,11}$/;
        if (!inputtxt.match(phoneno)) {
            if (!inputtxt.trim().match(land)) {
                return (true);
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }

    function IsWebsite(str) {
        regexp = /^(?:(?:https?|ftp):\/\/)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})))(?::\d{2,5})?(?:\/\S*)?$/;
        if (!regexp.test(str)) {
            return true;
        }
        else {
            return false;
        }
    }


    $('#btnAddOrganization').click(function () {
        if (Validate() == true) {
            var OrgID = $('#OrgID').val();
            var Name = $('#OrgName').val();
            var Address = $('#Address').val();
            var ContactPerson = $('#ContactPerson').val();
            var Phone = $('#Phone').val();         
            var State = $('#State').val();
            var Country = $('#Country').val();
            var Email = $('#Email').val();
            var Website = $('#Website').val();

            var data = new FormData();
            data.append("Organization_ID", OrgID);
            data.append("Organization_Name", Name);
            data.append("Organization_Address", Address);
            data.append("Organization_ContactPerson", ContactPerson);
            data.append("Organization_Phone", Phone);
            data.append("Organization_State", State);
            data.append("Organization_Country", Country);
            data.append("organization_Email", Email);
            data.append("organization_Web", Website);

            $.ajax({
                type: "POST",
                url: "/Master/SaveOrganization",
                data: data,
                contentType: false,
                processData: false,
                dataType: "json",
                beforeSend: function () { $('#loader').css("display", "block") },
                complete: function () { $('#loader').css("display", "none") },
                success: OnSuccessSaveCall,
                error: OnErrorSaveCall
            });

            function OnSuccessSaveCall(data) {
                if (data > "0") {
                    alert("Details have been submitted successfully.");
                    //$(window).scrollTop(0);
                    //$(".messagebox").append('<div class="well bg-success msg"><strong> Success!</strong> Save Successful.</div>');
                    //$(".msg").delay(8000).fadeOut(800);
                    //setTimeout(
                    //    function () {
                    //        location.href = "/Home/Dashboard";
                    //    }, 800);
                }
                else {
                    alert("Failed to submit details !");
                    //$(window).scrollTop(0);
                    //$(".messagebox").append('<div class="well bg-primary msg"><strong> Error!</strong> Failed To Save.</div>');
                    //$(".msg").delay(4000).fadeOut(800);
                }
            }

        function OnErrorSaveCall() {
            alert("Failed to submit details");
                //$(window).scrollTop(0);
                //$(".messagebox").append('<div class="well bg-primary msg"><strong> Error!</strong> Failed To Save.</div>');
                //$(".msg").delay(4000).fadeOut(800);
            }
        }
    });

});