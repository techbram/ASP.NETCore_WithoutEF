alert("Show Category");
//Category Modal
const _categoryModel = {
    Id: 0,
    Name: "",
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
                            $("<td>").text(category.id),
                            $("<td>").text(category.name),
                            $("<td>").text(category.createdDate),
                            $("<td>").append(
                                $("<button>").addClass("btn btn-primary btn-sm btn-edit-category").text("Edit").data("categorytData", category),
                                $("<button>").addClass("btn btn-danger btn-sm ms-2 btn-delete-category").text("Delete").data("categoryData", category)
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
// Function to get category name for product table
document.addEventListener("DOMContentLoaded", function () {

    alert("category Show!");

    CategoryListShow();   

    $("#createdDate").datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayHighlight: true
    })


}, false)