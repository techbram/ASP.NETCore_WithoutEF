alert("Show Category");
//Category Modal
const _categoryModel = {
    Id: 0,
    Name: "",
    DisplayOrder : 0,
    CreatedDate: ""
}

//Function to Category Table
function CategoryListShow() {
    //fetch the category list from the server
    fetch("/Category/GetListCategories")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                //clear the table body before appending new data
                $("#categoryTable tbody").html("");

                //Loop through each category and append to the table
                responseJson.forEach((category) => {
                    //append new row the table
                    $("#categoryTable tbody").append(
                        $("<tr>").append(                            
                            $("<td>").text(category.name),
                            $("<td>").text(category.displayOrder),
                            $("<td>").text(category.createdDate),
                            $("<td>").append(
                                $("<button>").addClass("btn btn-primary btn-sm btn-edit-category")
                                    .html('<i class="bi bi-pencil-square"></i> Edit')  // Add the icon with the text
                                    /*.text("Edit")*/
                                    .data("categorytData", category),
                                $("<button>").addClass("btn btn-danger btn-sm ms-2 btn-delete-category")
                                    .html('<i class="bi bi-trash-fill"></i> Edit') 
                                    /*.text("Delete")*/
                                    .data("categoryData", category)
                            )

                        )
                    );
                });
            }
            //add extra code by my self to catch error
            else {
                // Optional: Show a message if no category are available
                $("#categoryTable tbody").html("<tr><td colspan='4'>No category found</td></tr>");
            }
        })

        .catch(error => {
            console.error("Error fetching category data:", error);
            // Optional: Display an error message in case of failure
            $("#categoryTable tbody").html("<tr><td colspan='4'>Error loading category</td></tr>");
        });

}
// Function to get category name for category table
document.addEventListener("DOMContentLoaded", function () {

   /* alert("category Show!");*/

    CategoryListShow();   
    
    $("#dtCreated").datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayHighlight: true
    });

}, false)

//Function to Show modal of Category
function CategoryModalShow() {
    /*alert("Category Model Show!");*/
    
    $("#txtCategory").val(_categoryModel.Name);
    $("#displayOrder").val(_categoryModel.DisplayOrder);
    $("#dtCreated").val(_categoryModel.CreatedDate);

    $("#categoryModal").modal("show");
}

//Show new  modal on button click
$(document).on("click", ".btn-new-category", function () {
   /* alert("Category model!")*/
    _categoryModel.Id = 0;
    _categoryModel.Name = "";
    _categoryModel.DisplayOrder = 0;
    _categoryModel.CreatedDate = "";
    
    CategoryModalShow();
})

// Show edit product modal on button click
$(document).on("click", ".btn-edit-category", function () {
    const _category = $(this).data("categorytData");

    _categoryModel.Id = _category.id;
    _categoryModel.Name = _category.name;
    _categoryModel.DisplayOrder = _category.displayOrder;
    _categoryModel.CreatedDate = _category.createdDate;
    

    CategoryModalShow();
})

// Action for Save and Edit Category
$(document).on("click", ".btn-save-changes", function () {

    //const name = $("#txtCategory").val().trim();  // Get the trimmed value of the name
    //const displayOrder = $("#displayOrder").val().trim();  // Get the displayOrder value
    //const createdDate = $("#dtCreated").val();  // Get created date value

    //let valid = true; // Flag to check if the form is valid

    //// Clear previous error messages
    //$("#nameError").text('');
    //$("#displayOrderError").text('');

    //// Validate Name (required)
    //if (name === "") {
    //    $("#nameError").text("Name is required.");
    //    valid = false;
    //}

    //// Validate Display Order (required and should be a valid number)
    //if (displayOrder === "") {
    //    $("#displayOrderError").text("Display order is required.");
    //    valid = false;
    //} else if (isNaN(displayOrder) || displayOrder <= 0 || displayOrder >=101) {
    //    $("#displayOrderError").text("Display order must be between 1 to 100.");
    //    valid = false;
    //}

    //// If validation fails, do not send the request
    //if (!valid) {
    //    return; // Exit function and do not continue with the fetch request
    //}

    const model = {
        id: _categoryModel.Id,
        name: $("#txtCategory").val(),
        displayOrder: $("#displayOrder").val(),
        createdDate: $("#dtCreated").val()
    }

    // To Save
    if (_categoryModel.Id == 0) {

        fetch("/Category/SaveCategory", {
            method: "POST",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(model)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {
                if (responseJson.value) {
                    $("#categoryModal").modal("hide");
                    Swal.fire("Done!", "The category was created successfully", "success");
                    CategoryListShow();
                } else {
                    Swal.fire("Error!", "The category could not be created", "error");
                }
            })
    } else {    // To Edit

        console.log(_categoryModel.Id);
        fetch("/Category/EditCategory", {
            method: "PUT",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(model)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {
                if (responseJson.value) {
                    $("#categoryModal").modal("hide");
                    Swal.fire("Done", "The category was edited successfully", "success");
                    CategoryListShow();
                } else {
                    Swal.fire("Error", "The user category not be edited", "error");
                }
            })
    }
})

$(document).on("click", ".btn-delete-category", function () {

    const _category = $(this).data("categoryData");

    Swal.fire({
        title: "Are you sure?",
        text: `Delete Category "${_category.Name}"`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, back"
    }).then((result) => {

        if (result.isConfirmed) {

            fetch(`/Category/DeleteCategory?idCategory=${_category.id}`, {
                method: "DELETE"
            })
                .then(response => {
                    return response.ok ? response.json() : Promise.reject(response)
                })
                .then(responseJson => {
                    if (responseJson.value) {
                        Swal.fire("Done!", "The Category has be deleted", "success");
                        CategoryListShow();
                    }
                    else {
                        Swal.fire("Error!", "The Category could not be edited", "error");
                    }
                })
        }
    })
})