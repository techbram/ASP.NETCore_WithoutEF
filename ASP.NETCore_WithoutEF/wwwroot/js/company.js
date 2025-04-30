
const _companyModel = {
    Id: 0,
    Name: "",
    StreetAddress : "",
    City: "",
    State: "",
    PostalCode: "",
    PhoneNumber: ""
}

//Function to Company Table
function CompanyListShow() {
    //fetch the category list from the server
    fetch("/Admin/Company/GetListCompanies")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                //clear the table body before appending new data
                $("#companyTable tbody").html("");

                //Loop through each company and append to the table
                responseJson.forEach((company) => {
                    //append new row the table
                    $("#companyTable tbody").append(
                        $("<tr>").append(                            
                            $("<td>").text(company.name),
                            $("<td>").text(company.streetAddress),
                            $("<td>").text(company.city),
                            $("<td>").text(company.state),
                            $("<td>").text(company.postalCode),
                            $("<td>").text(company.phoneNumber),
                            $("<td>").append(
                                $("<button>").addClass("btn btn-primary btn-sm btn-edit-company")
                                    .html('<i class="bi bi-pencil-square"></i> Edit')  // Add the icon with the text
                                    /*.text("Edit")*/
                                    .data("companyData", company),
                                $("<button>").addClass("btn btn-danger btn-sm ms-2 btn-delete-company")
                                    .html('<i class="bi bi-trash-fill"></i> delete') 
                                    /*.text("Delete")*/
                                    .data("companyData", company)
                            )

                        )
                    );
                });
            }
            //add extra code by my self to catch error
            else {
                // Optional: Show a message if no company are available
                $("#companyTable tbody").html("<tr><td colspan='4'>No company found</td></tr>");
            }
        })

        .catch(error => {
            console.error("Error fetching company data:", error);
            // Optional: Display an error message in case of failure
            $("#companyTable tbody").html("<tr><td colspan='4'>Error loading company</td></tr>");
        });

}
// Function to get companies name for company table
document.addEventListener("DOMContentLoaded", function () {   

    CompanyListShow();

});

//Function to Show modal of Category
function CompanyModalShow() {
   
    $("#txtName").val(_companyModel.Name);
    $("#txtAddress").val(_companyModel.StreetAddress);
    $("#txtCity").val(_companyModel.City);
    $("#txtState").val(_companyModel.State);
    $("#txtPostalCode").val(_companyModel.PostalCode);
    $("#txtPhone").val(_companyModel.PhoneNumber);

    $("#companyModal").modal("show");
}

//Show new  modal on button click
$(document).on("click", ".btn-new-company", function () {
  
    _companyModel.Id = 0;
    _companyModel.Name = "";
    _companyModel.StreetAddress = "";
    _companyModel.City = "";
    _companyModel.State = "";
    _companyModel.PostalCode = "";
    _companyModel.PhoneNumber = "";

    CompanyModalShow();
})

// Show edit product modal on button click
$(document).on("click", ".btn-edit-company", function () {
    const _company = $(this).data("companyData");

    _companyModel.Id = _company.id;
    _companyModel.Name = _company.name;
    _companyModel.StreetAddress = _company.streetAddress;
    _companyModel.City = _company.city;
    _companyModel.State = _company.state;
    _companyModel.PostalCode = _company.postalCode;
    _companyModel.PhoneNumber = _company.phoneNumber;

    CompanyModalShow();
})

// Action for Save and Edit Category
$(document).on("click", ".btn-save-changes", function () {

    const model = {
        id: _companyModel.Id,
        name: $("#txtName").val(),
        streetAddress: $("#txtAddress").val(),
        city: $("#txtCity").val(),
        state: $("#txtState").val(),
        postalCode: $("#txtPostalCode").val(),
        phoneNumber: $("#txtPhone").val()
    }

    // To Save
    if (_companyModel.Id == 0) {

        fetch("/Admin/Company/SaveCompany", {
            method: "POST",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(model)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {
                if (responseJson.value) {
                    $("#companyModal").modal("hide");
                    Swal.fire("Done!", "The company was created successfully", "success");
                    CompanyListShow();
                } else {
                    Swal.fire("Error!", "The company could not be created", "error");
                }
            })
    } else {    // To Edit

        console.log(_companyModel.Id);
        fetch("/Admin/Company/EditCompany", {
            method: "PUT",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(model)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {
                if (responseJson.value) {
                    $("#companyModal").modal("hide");
                    Swal.fire("Done", "The company was edited successfully", "success");
                    CompanyListShow();
                } else {
                    Swal.fire("Error", "The user company not be edited", "error");
                }
            })
    }
})

$(document).on("click", ".btn-delete-company", function () {

    const _company = $(this).data("companyData");

    Swal.fire({
        title: "Are you sure?",
        text: `Delete Company "${_company.name}"`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, back"
    }).then((result) => {

        if (result.isConfirmed) {

            fetch(`/Admin/Company/DeleteCompany?idCompany=${_company.id}`, {
                method: "DELETE"
            })
                .then(response => {
                    return response.ok ? response.json() : Promise.reject(response)
                })
                .then(responseJson => {
                    if (responseJson.value) {
                        Swal.fire("Done!", "The Company has be deleted", "success");
                        CompanyListShow();
                    }
                    else {
                        Swal.fire("Error!", "The Company could not be edited", "error");
                    }
                })
        }
    })
})