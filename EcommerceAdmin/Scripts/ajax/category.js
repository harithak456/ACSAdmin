$(document).ready(function () {
    $(".btnAddRow").bind("click", AddMorRow);  
    $(".btnDeleteRow").bind("click", DeleteRow); //Dynamic Remove Button in EM

    var tableSub = 1;
    function AddMorRow() {
        tableSub = tableSub + 1;
        var newDiv = '<tr class="Sub' + tableSub + ' tblRow"> ' +
            '<td><input type="text" class="form-control subCategory" placeholder="Enter SubCategory"></td>' +           
            '<td><button type="button" class="btn btn-primary btnAddRow' + tableSub + '">ADD</button> ' +
            '<button type="button" class="btn btn-primary btnDeleteRow' + tableSub + '">REMOVE</button><input type="hidden" value="0" /></td ></tr > ';
        $('#tblSubCategoryBody').append(newDiv);

        $(".btnAddRow" + tableSub).bind("click", AddMorRow);
        $(".btnDeleteRow" + tableSub).bind("click", DeleteRow);      
    }

    function DeleteRow() {
        if ($('#tblSubCategory tr').length > 2) {
            $(this).parent().parent().remove();
        }
        else {
            $(this).closest("tr").find("input[class='subCategory']").val("");
        }
    }

    function Validate() {
        var flag = true;
        $(".inputValid").html("");      
        var CategoryName = $("#CategoryName").val();

        if (CategoryName == null || CategoryName.trim() == "") {
            $("#validCategory").html("This field is required.");
            flag = false;
        }       
        return flag;
    }

    $(".btnSubmit").click(function () {
        if (Validate()) {
        var CategoryID = $("#CategoryID").val();
            var CategoryName = $("#CategoryName").val();
     
            var SubList = new Array();        
            $("#tblSubCategoryBody .tblRow").each(function () {
                if ($(this).find(".subCategory").val() != "") {
                    var SubCategoryModel = new Object();                   
                    SubCategoryModel.Category_Name = $(this).find(".subCategory").val();                                                      
                    SubCategoryModel.Category_ID = $(this).find("#subCategoryID").val();                                                      
                    SubList.push(SubCategoryModel);
                }
            });
       
            var data = {
                "Category_ID": CategoryID,         
                "Category_Name": CategoryName,         
                "SubCategoryList": SubList
            };          
            $.ajax({
                type: "POST",
                url: "/Category/SaveCategory",
                data: JSON.stringify({ "model": data }),              
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () { $("#loader").css("display", "block"); },
                complete: function () { $("#loader").css("display", "none"); },
                success: OnSuccessSaveCall,
                error: OnErrorSaveCall
            });
         
            function OnSuccessSaveCall(data) {
                if (data > 0) {
                    alert("Details have been submitted successfully.");
                    //$(".messagebox").append('<div class="alert alert-success msg"><strong> Success!</strong> Save successful.</div>');
                    //$(".msg").delay(4000).fadeOut(800);
                    //if (DietID > 0) {
                    //    alert("Updated Success");
                    //}
                    //else {
                    //    alert("Saved Success");
                    //}
                    location.href = "/Master/Category";
                }
                else if (data == 0) {
                    alert("Failed to submit details !");
                    //$(".messagebox").append('<div class="alert alert-danger msg"><strong> Error!</strong> Same record already exists.</div>');
                    //$(".msg").delay(4000).fadeOut(800);
                }
                else {
                    alert("Failed to submit details !");
                    //$(".messagebox").append('<div class="alert alert-danger msg"><strong> Error!</strong> Please try again.</div>');
                    //$(".msg").delay(4000).fadeOut(800);
                }
            }
        function OnErrorSaveCall() {
            alert("Failed to submit details !");
                //$(".messagebox").append('<div class="alert alert-danger msg"><strong> Error!</strong> Please try again.</div>');
                //$(".msg").delay(4000).fadeOut(800);
            }
        }
        //else {
        //    $(".messagebox").append('<div class="alert alert-danger msg"><strong> Error!</strong> Please fill mandatory fields.</div>');
        //    $(".msg").delay(4000).fadeOut(800);
        //}
        //$("html, body").animate({ scrollTop: 0 }, "slow");
    });

    $(".btnDeleteCategory").click(function () {       
        var row = $(this).closest('tr');
        var SubCategoryCount = $(row).find('td:eq(2)').text();      
        if (SubCategoryCount == 0) {
            if (confirm("Are You Sure You Want To Delete?")) {
              
                var Category_ID = $(row).find('td:eq(3)').find('input').val();
                $.ajax({
                    type: "POST",
                    url: "/Category/DeleteCategory",
                    data: "{'Category_ID':'" + Category_ID + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function () { $("#loader").css("display", "block"); },
                    complete: function () { $("#loader").css("display", "none"); },
                    success: OnSuccessSaveCall,
                    error: OnErrorSaveCall
                });
                function OnSuccessSaveCall(data) {
                    if (data > 0) {
                        //$("#" + Category_ID).fadeOut("slow").remove();
                        location.reload();
                        //$(window).scrollTop(0);
                        //$(".messagebox").append('<div class="well bg-success msg"><strong> Success!</strong> Delete Successful.</div>');
                        //$(".msg").delay(4000).fadeOut(800);
                    }
                    else {
                        alert("Failed To Delete Data");
                        $(window).scrollTop(0);
                        //$(".messagebox").append('<div class="well bg-primary msg"><strong> Error!</strong> Failed To Delete.</div>');
                        //$(".msg").delay(4000).fadeOut(800);
                    }
                }
                function OnErrorSaveCall() {
                    alert("Failed To Delete Data");
                    $(window).scrollTop(0);
                    //$(".messagebox").append('<div class="well bg-primary msg"><strong> Error!</strong> Failed To Delete.</div>');
                    //$(".msg").delay(4000).fadeOut(800);
                }
            }
        }
        else {
            alert("Sorry we could'nt delete this category because it has Subcategories");
            $(window).scrollTop(0);
        }
    });
});