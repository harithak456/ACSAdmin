$(document).ready(function () {

    function Validate() {
        var flag = true;
        $(".inputValid").html("");
        var Name = $('#Name').val(); 
        var Price = $('#Price').val();
        var Description = $('#Description').val();
        var Category = $('#Category option:selected').val();
        var SubCategory = $('#SubCategory option:selected').val();
        var Brand = $('#Brand option:selected').val();

        if (Name == null || Name.trim() == "") {
            $("#validName").html("This field is required.");
            flag = false;
        }
        if (Description == null || Description.trim() == "") {
            $("#validDescription").html("This field is required.");
            flag = false;
        }
        if (Price == null || Price.trim() == "" || Price ==0) {
            $("#validPrice").html("This field is required.");
            flag = false;
        }
        if (Category == 0) {
            $("#validCategory").html("This field is required.");
            flag = false;
        }
        if (SubCategory == 0) {
            $("#validSubCategory").html("This field is required.");
            flag = false;
        }
        if (Brand == 0) {
            $("#validBrand").html("This field is required.");
            flag = false;
        }

       
        return flag;
    }

    $('#btnSave').click(function () {      
        if (Validate()) {
        var ProductID = $('#ProductID').val();
        var Name = $('#Name').val(); 
        var SubText = $('#SubText').val();
        var Category = $('#Category option:selected').val();
        var SubCategory = $('#SubCategory option:selected').val();
        var Brand = $('#Brand option:selected').val();
        var Price = $('#Price').val();
        var Discount = $('#Discount').val();
        var Description = $('#Description').val();
        var image = $('#inputImage').get(0).files;

        var data = new FormData();
        data.append("Product_ID", ProductID);
        data.append("Product_Name", Name);
        data.append("Product_SubText", SubText);
        data.append("Category_ID", Category);
        data.append("SubCategory_ID", SubCategory);
        data.append("Brand_ID", Brand);
        data.append("Product_Price", Price);
        data.append("Product_Discount", Discount);
        data.append("Product_Description", Description);
        data.append("Product_ImageFile", image[0]);
        data.append("Product_Image", Name+".jpg");

            $.ajax({
                type: "POST",
                url: "/Product/SaveProduct",
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
                    alert("Product Details Saved Successfully");
                    location.href="/Master/Products";
                    //$(window).scrollTop(0);
                    //$(".messagebox").append('<div class="well bg-success msg"><strong> Success!</strong> Save Successful.</div>');
                    //$(".msg").delay(8000).fadeOut(800);

                }
                else {
                    alert("Failed To Save Product Details");
                    //$(window).scrollTop(0);
                    //$(".messagebox").append('<div class="well bg-primary msg"><strong> Error!</strong> Failed To Save.</div>');
                    //$(".msg").delay(4000).fadeOut(800);  
                }
            }
            function OnErrorSaveCall() {
                alert("Failed To Save Product Details");
                //$(window).scrollTop(0);
                //$(".messagebox").append('<div class="well bg-primary msg"><strong> Error!</strong> Failed To Save.</div>');
                //$(".msg").delay(4000).fadeOut(800);
            }
        }
    });

    //Category Change
    $('#Category').change(function () {
        
        $('#SubCategory').empty();

        var Category = $(this).val();    

        $.ajax({
            type: "POST",
            url: "/Category/SelectSubCategory",
            data: "{'Category':'" + Category + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            //beforeSend: function () { $("#loader").css("display", "block"); },
            //complete: function () { $("#loader").css("display", "none"); },
            success: function (data) {
                var txt = "";
                if (Object.keys(data).length > 0) {
                    $.each(data, function (index, dbval) {
                        txt = "<option value='" + dbval.Category_ID + "'>" + dbval.Category_Name + "</option>";
                        $('#SubCategory').append(txt);
                    });
                }
                else {
                    $('#SubCategory').empty();
                    var txt1 = "<option value=''>--SELECT--</option>";
                    $('#SubCategory').append(txt1);
                }

            },
            error: function () {
                $('#SubCategory').empty();
                var txt1 = "<option value=''>--SELECT--</option>";
                $('#SubCategory').append(txt1);}
        });
    });

    $(".btnDeleteProduct").click(function () {
        if (confirm("Are You Sure You Want To Delete?")) {
            var row = $(this).closest('tr');
            var ProductId = $(row).find('td:eq(3)').find('input').val();
            $.ajax({
                type: "POST",
                url: "/Product/DeleteProduct",
                data: "{'Product_ID':'" + ProductId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () { $("#loader").css("display", "block"); },
                complete: function () { $("#loader").css("display", "none"); },
                success: OnSuccessSaveCall,
                error: OnErrorSaveCall
            });
            function OnSuccessSaveCall(data) {
                if (data > 0) {
                    $("#" + ProductId).fadeOut("slow").remove();
                    location.reload();
                    //$(window).scrollTop(0);
                    //$(".messagebox").append('<div class="well bg-success msg"><strong> Success!</strong> Delete Successful.</div>');
                    //$(".msg").delay(4000).fadeOut(800);
                }
                else {
                    //$(window).scrollTop(0);
                    //$(".messagebox").append('<div class="well bg-primary msg"><strong> Error!</strong> Failed To Delete.</div>');
                    //$(".msg").delay(4000).fadeOut(800);
                }
            }
            function OnErrorSaveCall() {
                //$(window).scrollTop(0);
                //$(".messagebox").append('<div class="well bg-primary msg"><strong> Error!</strong> Failed To Delete.</div>');
                //$(".msg").delay(4000).fadeOut(800);
            }
        }
    });
});