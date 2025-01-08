alert("Hello, World!"); 
// Product Modal
const _productModel = {
    Id: 0,
    ProductName: "",
    CategoryRef: 0,
    Price: 0
    /*contractDate: ""*/
}

// Function to Products Table
function ProductsShow() {
    alert("Products Show!");
    fetch("/Home/GetListProducts")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.length > 0) {

                $("#productsTable tbody").html("");


                responseJson.forEach((product) => {
                    $("#productsTable tbody").append(
                        $("<tr>").append(
                            $("<td>").text(product.productName),
                            $("<td>").text(product.categoryRef.name),
                            $("<td>").text(product.price),
                           /* $("<td>").text(employee.contractDate),*/
                            $("<td>").append(
                                $("<button>").addClass("btn btn-primary btn-sm btn-edit-product").text("Edit").data("productData", product),
                                $("<button>").addClass("btn btn-danger btn-sm ms-2 btn-delete-product").text("Delete").data("productData", product)
                            )
                        )
                    )
                })
            }
        })
}

// Function to get category name for product table
document.addEventListener("DOMContentLoaded", function () {
    
    ProductsShow();

    fetch("/Home/GetListCategories")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {

            if (responseJson.length > 0) {
                responseJson.forEach((category) => {

                    $("#selectCategories").append(
                        $("<option>").val(category.id).text(category.name)
                    )

                })
            }

        })

    //$("#txtContractDate").datepicker({
    //    format: "dd/mm/yyyy",
    //    autoclose: true,
    //    todayHighlight: true
    //})


}, false)

// Function to Show modal
//function ModalShow() {

//    $("#txtName").val(_productModel.productName);
//    $("#selectCategory").val(_productModel.categoryRef == 0 ? $("#selectCategory option:first").val() : _productModel.categoryRef);
//    $("#numSalary").val(_productModel.price);
//    /*$("#txtContractDate").val(_employeeModel.contractDate);*/

//    $("#productModal").modal("show");
//}