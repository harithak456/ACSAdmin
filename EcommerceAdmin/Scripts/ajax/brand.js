$(document).ready(function () {   

    $('#btnCancel').click(function () {
        $('#BrandID').val("")
        $('#BrandName').val("");      
    });
    $('#btnSave').click(function () {
        //  FillTable(); 
        $(".inputValid").html("");
        var BrandName = $('#BrandName').val();     
        if (BrandName != null && BrandName.trim() != "") {          
            var BrandID = $('#BrandID').val();
            var data = new FormData();
            data.append("Brand_ID", BrandID);
            data.append("Brand_Name", BrandName);

            $.ajax({
                type: "POST",
                url: "/Category/SaveBrand",
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
                    // FillTable();

                    location.reload();
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
        else {
            $("#validBrand").html("This field is required.");
        }
    });

    function FillTable() {
        $('#tbBrandBody').empty();

        $.ajax({
            type: "POST",
            url: "/Master/SelectBrandList",
            data: "",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () { $("#loader").css("display", "block"); },
            complete: function () { $("#loader").css("display", "none"); },
            success: function (data) {
                var txt = "";
                if (Object.keys(data).length > 0) {
                    $.each(data, function (index, dbval) {
                        index++;
                        //txt = "<tr id='" + dbval.Brand_ID + "'><td>" + index + "</td><td>" + dbval.Brand_Name + "</td>" +
                        //    " <td><div class='tools'><a class='' onclick='EditBrand(" + dbval.Brand_ID + ",'" + dbval.Brand_Name + "')'>" +
                        //    "<i class='fa fa-edit' ></i></a> <a href='' class='btnDeleteBrand'>  <i class='fa fa-trash-o'></i></a>" +
                        //    "<input type='hidden' value='" + dbval.Brand_ID + "' /></div></td> </tr>";


                        txt = "<tr id = '" + dbval.Brand_ID + "'><td>" + index + "</td><td>" + dbval.Brand_Name + "</td><td>" +
                            "<div class='tools'> <a class='' onclick='EditBrand(" + dbval.Brand_ID + ",'asd')'><i class='fa fa-edit' ></i>" +
                            " </a><a href='' class='btnDeleteBrand'>  <i class='fa fa-trash-o'></i></a><input type='hidden' value='@item.Brand_ID' />" +
                            "</div> </td></tr>";
                        console.log(txt);
                        $('#tbBrandBody').append(txt);
                    });
                }               
            },
            error: function () { }
        });
    }   

    $(".btnDeleteBrand").click(function () {
        if (confirm("Are You Sure You Want To Delete?")) {
            var row = $(this).closest('tr');
            var brandid = $(row).find('td:eq(2)').find('input').val();
            $.ajax({
                type: "POST",
                url: "/Master/DeleteBrand",
                data: "{'Brand_ID':'" + brandid + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () { $("#loader").css("display", "block"); },
                complete: function () { $("#loader").css("display", "none"); },
                success: OnSuccessSaveCall,
                error: OnErrorSaveCall
            });
            function OnSuccessSaveCall(data) {
                if (data > 0) {
                    $("#" + brandid).fadeOut("slow").remove();
                    $(window).scrollTop(0);
                    $(".messagebox").append('<div class="well bg-success msg"><strong> Success!</strong> Delete Successful.</div>');
                    $(".msg").delay(4000).fadeOut(800);
                }
                else {
                    $(window).scrollTop(0);
                    $(".messagebox").append('<div class="well bg-primary msg"><strong> Error!</strong> Failed To Delete.</div>');
                    $(".msg").delay(4000).fadeOut(800);
                }
            }
            function OnErrorSaveCall() {
                $(window).scrollTop(0);
                $(".messagebox").append('<div class="well bg-primary msg"><strong> Error!</strong> Failed To Delete.</div>');
                $(".msg").delay(4000).fadeOut(800);
            }
        }
    });
});