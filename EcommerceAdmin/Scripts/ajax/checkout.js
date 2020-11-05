$(document).ready(function () {
    $('#btnProceed').click(function () {    
        //if (Validate() == true) {
        var FName = $('#FName').val();
        var LName = $('#LName').val();
        var Address1 = $('#Address1').val();
        var Address2 = $('#Address2').val();
        var Town = $('#Town').val();
        var State = $('#State').val();
            var Country = $('#Country').val();
            var Email = $('#Email').val();
        var Phone = $('#Phone').val();

            var data = new FormData();
        data.append("Guest_FirstName", FName);
        data.append("Guest_LastName", LName);
        data.append("Guest_Address1", Address1);
        data.append("Guest_Address2", Address2);
        data.append("Guest_Town", Town);
        data.append("Guest_State", State);
        data.append("Guest_Country", Country);
        data.append("Guest_Email", Email);
        data.append("Guest_Phone", Phone);

            $.ajax({
                type: "POST",
                url: "/Order/SaveOrder",
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
                    alert("Success");
                  
                }
                else {
                    alert("Failed");                   
                }
            }

            function OnErrorSaveCall() {
                alert("Failed");
              
            }
        //}
    });
});