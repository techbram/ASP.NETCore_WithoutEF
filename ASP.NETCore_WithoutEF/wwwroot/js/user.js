
const _userModel = {
    Name: "",
    Email: "",
    PhoneNumber: "",
    CompanyId: 0,
    RoleId: ""

};

//Function to Company Table
function UserListShow() {
    //fetch the category list from the server
    fetch("/Admin/User/GetUsers")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                //clear the table body before appending new data
                $("#userTable tbody").html("");

                //Loop through each company and append to the table
                responseJson.forEach((user) => {
                    //append new row the table
                    $("#userTable tbody").append(
                        $("<tr>").append(                            
                            $("<td>").text(user.name),
                            $("<td>").text(user.email),
                            $("<td>").text(user.phoneNumber),
                            $("<td>").text(user.companyRef.name),
                            $("<td>").text(user.roleRef.name),                           
                            $("<td>").append(
                                $("<button>").addClass("btn btn-primary btn-sm btn-edit-user")
                                    .html('<i class="bi bi-pencil-square"></i> Edit')  // Add the icon with the text
                                    /*.text("Edit")*/
                                    .data("userData", user)
                            )

                        )
                    );
                });
            }
            //add extra code by my self to catch error
            else {
                // Optional: Show a message if no company are available
                $("#userTable tbody").html("<tr><td colspan='4'>No user found</td></tr>");
            }
        })

        .catch(error => {
            console.error("Error fetching user data:", error);
            // Optional: Display an error message in case of failure
            $("#userTable tbody").html("<tr><td colspan='4'>Error loading user</td></tr>");
        });

}
// Function to get companies name for company table
document.addEventListener("DOMContentLoaded", function () {   

    UserListShow();


});

////Function to Show modal of Category
//function CompanyModalShow() {
   
//    $("#txtName").val(_companyModel.Name);
//    $("#txtAddress").val(_companyModel.StreetAddress);
//    $("#txtCity").val(_companyModel.City);
//    $("#txtState").val(_companyModel.State);
//    $("#txtPostalCode").val(_companyModel.PostalCode);
//    $("#txtPhone").val(_companyModel.PhoneNumber);

//    $("#companyModal").modal("show");
//}

////Show new  modal on button click
//$(document).on("click", ".btn-new-company", function () {
  
//    _companyModel.Id = 0;
//    _companyModel.Name = "";
//    _companyModel.StreetAddress = "";
//    _companyModel.City = "";
//    _companyModel.State = "";
//    _companyModel.PostalCode = "";
//    _companyModel.PhoneNumber = "";

//    CompanyModalShow();
//})

//// Show edit product modal on button click
//$(document).on("click", ".btn-edit-company", function () {
//    const _company = $(this).data("companyData");

//    _companyModel.Id = _company.id;
//    _companyModel.Name = _company.name;
//    _companyModel.StreetAddress = _company.streetAddress;
//    _companyModel.City = _company.city;
//    _companyModel.State = _company.state;
//    _companyModel.PostalCode = _company.postalCode;
//    _companyModel.PhoneNumber = _company.phoneNumber;

//    CompanyModalShow();
//})

//// Action for Save and Edit Category
//$(document).on("click", ".btn-save-changes", function () {

//    const model = {
//        id: _companyModel.Id,
//        name: $("#txtName").val(),
//        streetAddress: $("#txtAddress").val(),
//        city: $("#txtCity").val(),
//        state: $("#txtState").val(),
//        postalCode: $("#txtPostalCode").val(),
//        phoneNumber: $("#txtPhone").val()
//    }

//    // To Save
//    if (_companyModel.Id == 0) {

//        fetch("/Admin/Company/SaveCompany", {
//            method: "POST",
//            headers: { "Content-Type": "application/json; charset=utf-8" },
//            body: JSON.stringify(model)
//        })
//            .then(response => {
//                return response.ok ? response.json() : Promise.reject(response);
//            })
//            .then(responseJson => {
//                if (responseJson.value) {
//                    $("#companyModal").modal("hide");
//                    Swal.fire("Done!", "The company was created successfully", "success");
//                    CompanyListShow();
//                } else {
//                    Swal.fire("Error!", "The company could not be created", "error");
//                }
//            })
//    } else {    // To Edit

//        console.log(_companyModel.Id);
//        fetch("/Admin/Company/EditCompany", {
//            method: "PUT",
//            headers: { "Content-Type": "application/json; charset=utf-8" },
//            body: JSON.stringify(model)
//        })
//            .then(response => {
//                return response.ok ? response.json() : Promise.reject(response)
//            })
//            .then(responseJson => {
//                if (responseJson.value) {
//                    $("#companyModal").modal("hide");
//                    Swal.fire("Done", "The company was edited successfully", "success");
//                    CompanyListShow();
//                } else {
//                    Swal.fire("Error", "The user company not be edited", "error");
//                }
//            })
//    }
//})

//$(document).on("click", ".btn-delete-company", function () {

//    const _company = $(this).data("companyData");

//    Swal.fire({
//        title: "Are you sure?",
//        text: `Delete Company "${_company.name}"`,
//        icon: "warning",
//        showCancelButton: true,
//        confirmButtonColor: "#3085d6",
//        cancelButtonColor: "#d33",
//        confirmButtonText: "Yes, delete it!",
//        cancelButtonText: "No, back"
//    }).then((result) => {

//        if (result.isConfirmed) {

//            fetch(`/Admin/Company/DeleteCompany?idCompany=${_company.id}`, {
//                method: "DELETE"
//            })
//                .then(response => {
//                    return response.ok ? response.json() : Promise.reject(response)
//                })
//                .then(responseJson => {
//                    if (responseJson.value) {
//                        Swal.fire("Done!", "The Company has be deleted", "success");
//                        CompanyListShow();
//                    }
//                    else {
//                        Swal.fire("Error!", "The Company could not be edited", "error");
//                    }
//                })
//        }
//    })
//})