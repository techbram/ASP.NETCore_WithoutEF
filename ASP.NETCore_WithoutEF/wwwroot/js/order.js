

// Function to Products Table
function OrderListShow(status) {
    //Fetch product data from the server
    fetch("/Admin/Order/GetListOrderHeaders?status=" + status)
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
                                $("<a>").attr("href", `/Admin/Order/Detail?orderId=${orderHeader.id}`)
                                .addClass("btn btn-primary btn-sm btn-edit-orderHeader ").text("Edit").data("orderHeaderData", orderHeader)
                               
                            )
                        )
                    );
                });
            }
            //added extra code by my self to catch error
            else {
                // Optional: Show a message if no products are available
                $("#orderHeaderTable tbody").html("<tr><td class='text-center' colspan='7' >No Order found</td></tr>");
            }
        })
        .catch(error => {
            console.error("Error fetching product data:", error);
            // Optional: Display an error message in case of failure
            $("#orderHeaderTable tbody").html("<tr><td colspan='7'>Error loading orders</td></tr>");
        });
}

// Function to get category name for product table
document.addEventListener("DOMContentLoaded", function () {

   /* OrderListShow();*/

    var url = window.location.search;
    if (url.includes("inprocess")) {
        OrderListShow("inprocess");
    }
    else {
        if (url.includes("completed")) {
            OrderListShow("completed");
        }
        else {
            if (url.includes("pending")) {
                OrderListShow("pending");
            }
            else {
                if (url.includes("approved")) {
                    OrderListShow("approved");
                }
                else {
                    OrderListShow("all");
                }
            }
        }
    }
}, false);