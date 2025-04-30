/*alert("Hello, World!"); */
// Product Modal
const _productModel = {
    Id: 0,
    ProductName: "",
    ProductNumber: "",
    Originated : "",
    CategoryRef: 0,
    ProductDetail: "",
    Price: 0,
    Price50: 0,
    Price100: 0,
    DiscountPrice: 0,
    DateAdded: "",
    ImageUrl: ""
    /*contractDate: ""*/
}

// Function to Products Table
function ProductsShow() {
   //Fetch product data from the server
    fetch("/Admin/Product/GetListProducts")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                // Clear the table body before appending new data
                $("#productsTable tbody").html("");

                // Loop through each product and append to the table
                responseJson.forEach((product) => {
                    // Append new row to the table
                    $("#productsTable tbody").append(
                        $("<tr>").append(
                            $("<td>").append(
                                $("<a>").attr("href", `/Admin/Product/Detail?id=${product.id}`) 
                                    .text(product.productName)
                                    .data("productData", product)
                            ),
                            /*$("<td>").text(product.productName),*/
                            $("<td>").text(product.categoryRef.name),
                            $("<td>").text(product.price),  //Formate price to 2 decimal places                           
                            $("<td>").text(product.discountPrice),
                            $("<td>").text(product.dateAdded),
                            $("<td>").append(
                                $("<button>").addClass("btn btn-primary btn-sm btn-edit-product ").text("Edit").data("productData", product),
                                $("<button>").addClass("btn btn-danger btn-sm ms-2 btn-delete-product ").text("Delete").data("productData", product)
                            )
                        )
                    );
                });
            }
             //added extra code by my self to catch error
            else {
                // Optional: Show a message if no products are available
                $("#productsTable tbody").html("<tr><td colspan='4'>No products found</td></tr>");
            }
        })
        .catch(error => {
            console.error("Error fetching product data:", error);
            // Optional: Display an error message in case of failure
            $("#productsTable tbody").html("<tr><td colspan='4'>Error loading products</td></tr>");
        });
}

// Function to get category name for product table
document.addEventListener("DOMContentLoaded", function () {
    
    ProductsShow();

    fetch("/Admin/Product/GetListCategories")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {

            if (responseJson.length > 0) {
                responseJson.forEach((category) => {

                    $("#selectCategory").append(
                        $("<option>").val(category.id).text(category.name)
                    );

                })
            }

        })
        .catch(error => {
            console.error('Error fetching categories:', error);
            alert('Failed to load categories');
        });

    $("#txtDate").datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayHighlight: true
    });


}, false)

 //Function to Show modal
function ModalShow() {
   
    $("#txtName").val(_productModel.ProductName);
    $("#txtProductNum").val(_productModel.ProductNumber);
    $("#txtOrigin").val(_productModel.Originated);
    $("#selectCategory").val(_productModel.CategoryRef == 0 ? $("#selectCategory option:first").val() : _productModel.CategoryRef);
    $("#txtDetail").val(_productModel.ProductDetail);
    $("#numPrice").val(_productModel.Price);
    $("#numPrice50").val(_productModel.Price50);
    $("#numPrice100").val(_productModel.Price100);
    $("#numDiscount").val(_productModel.DiscountPrice);
    $("#txtDate").val(_productModel.DateAdded);
    $("#txtImage").val(_productModel.ImageUrl);
   
    $("#productModal").modal("show");
}

//Show new  modal on button click
$(document).on("click", ".btn-new-product", function () {
    alert("Product model!")
    _productModel.Id = 0;
    _productModel.ProductName = "";
    _productModel.ProductNumber = "";
    _productModel.Originated = "";
    _productModel.CategoryRef = 0;
    _productModel.ProductDetail = "";
    _productModel.Price = 0;
    _productModel.Price50 = 0;
    _productModel.Price100 = 0;   
    _productModel.DiscountPrice = 0;
    _productModel.DateAdded = "";
    _productModel.ImageUrl = "";

    ModalShow();
})


// Show edit product modal on button click
$(document).on("click", ".btn-edit-product", function () {
    const _product = $(this).data("productData");

    _productModel.Id = _product.id;
    _productModel.ProductName = _product.productName;
    _productModel.ProductNumber = _product.productNumber;
    _productModel.Originated = _product.originated;
    _productModel.CategoryRef = _product.categoryRef.id;
    _productModel.ProductDetail = _product.productDetail;
    _productModel.Price = _product.price;
    _productModel.Price50 = _product.price50;
    _productModel.Price100 = _product.price100;    
    _productModel.DiscountPrice = _product.discountPrice;
    _productModel.DateAdded = _product.dateAdded;
    _productModel.ImageUrl = _product.imageUrl;

    ModalShow();
})


// Action for Save and Edit product
$(document).on("click", ".btn-save-changes", function () {

    const model = {
        id: _productModel.Id,
        productName: $("#txtName").val(),
        productNumber: $("#txtProductNum").val(),
        originated: $("#txtOrigin").val(),
        categoryRef: {
            id: $("#selectCategory").val()
        },
        productDetail: $("#txtDetail").val(),
        price: $("#numPrice").val(),    
        price50: $("#numPrice50").val(),
        price100: $("#numPrice100").val(),
        discountPrice: $("#numDiscount").val(),
        dateAdded: $("#txtDate").val(),  
        imageUrl: $("#txtImage").val(),
    }

    // To Save
    if (_productModel.Id == 0) {

        fetch("/Admin/Product/SaveProduct", {
            method: "POST",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(model)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {
                if (responseJson.value) {
                    $("#productModal").modal("hide");
                    Swal.fire("Done!", "The product was created successfully", "success");
                    ProductsShow();
                } else {
                    Swal.fire("Error!", "The product could not be created", "error");
                }
            })
    } else {    // To Edit

        console.log(_productModel.Id);
        fetch("/Admin/Product/EditProduct", {
            method: "PUT",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(model)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {
                if (responseJson.value) {
                    $("#productModal").modal("hide");
                    Swal.fire("Done", "The product was edited successfully", "success");
                    ProductsShow();
                } else {
                    Swal.fire("Error", "The user product not be edited", "error");
                }
            })
    }
})

// Show alert delete employee on button click
$(document).on("click", ".btn-delete-product", function () {

    const _product = $(this).data("productData");

    Swal.fire({
        title: "Are you sure?",
        text: `Delete product "${_product.productName}"`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, back"
    }).then((result) => {

        if (result.isConfirmed) {

            fetch(`/Admin/Product/DeleteProduct?idProduct=${_product.id}`, {
                method: "DELETE"
            })
                .then(response => {
                    return response.ok ? response.json() : Promise.reject(response)
                })
                .then(responseJson => {
                    if (responseJson.value) {
                        Swal.fire("Done!", "The product has be deleted", "success");
                        ProductsShow();
                    }
                    else {
                        Swal.fire("Error!", "The product could not be edited", "error");
                    }
                })
        }
    })
})