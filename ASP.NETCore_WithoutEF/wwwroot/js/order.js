/*alert("Hello, World!"); */
// Product Modal
const _orderHeaderModel = {
    Id: 0,
    Name: "",
    PhoneNumber: "",
    Email: "",
    Status: "",
    Total: 0,
   
}

// Function to Products Table
function OrderListShow() {
    //Fetch product data from the server
    fetch("/Order/GetListOrderHeaders")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                // Clear the table body before appending new data
                $("#orderHeaderTable tbody").html("");
                
                // Loop through each order and append to the table
                responseJson.forEach((orderHeader) => {
                    // Append new row to the table
                    $("#orderHeaderTable tbody").append(
                        $("<tr>").append(
                            $("<td>").text(orderHeader.id),
                            $("<td>").text(orderHeader.name),
                            $("<td>").text(orderHeader.phoneNumber),  //Formate price to 2 decimal places                           
                            $("<td>").text(orderHeader.applicationUser.email),
                            $("<td>").text(orderHeader.orderStatus),
                            $("<td>").text(orderHeader.orderTotal),
                            $("<td>").append(
                                $("<button>").addClass("btn btn-primary btn-sm btn-edit-orderHeader ").text("Edit").data("orderHeaderData", orderHeader),
                               
                            )
                        )
                    );
                });
            }
            //added extra code by my self to catch error
            else {
                // Optional: Show a message if no products are available
                $("#orderHeaderTable tbody").html("<tr><td colspan='4'>No Order found</td></tr>");
            }
        })
        .catch(error => {
            console.error("Error fetching product data:", error);
            // Optional: Display an error message in case of failure
            $("#orderHeaderTable tbody").html("<tr><td colspan='4'>Error loading orders</td></tr>");
        });
}

// Function to get category name for product table
document.addEventListener("DOMContentLoaded", function () {

    OrderListShow();

    //fetch("/Order/GetListCategories")
    //    .then(response => {
    //        return response.ok ? response.json() : Promise.reject(response)
    //    })
    //    .then(responseJson => {

    //        if (responseJson.length > 0) {
    //            responseJson.forEach((category) => {

    //                $("#selectCategory").append(
    //                    $("<option>").val(category.id).text(category.name)
    //                );

    //            })
    //        }

    //    })
    //    .catch(error => {
    //        console.error('Error fetching categories:', error);
    //        alert('Failed to load categories');
    //    });

    //$("#txtDate").datepicker({
    //    format: "dd/mm/yyyy",
    //    autoclose: true,
    //    todayHighlight: true
    //});


}, false)