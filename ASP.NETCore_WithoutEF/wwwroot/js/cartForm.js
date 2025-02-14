
$("#saveCartForm").submit(function (e) {
    alert("test");
    e.preventDefault(); // Prevent default form submission

    var formData = $(this).serialize(); // Serialize form data

    $.ajax({
        type: "POST",
        url: '@Url.Action("Detail", "Home")', // Replace with your controller's name
        data: formData,
        success: function (response) {
            // Check if the response is successful
            if (response.success) {
                // SweetAlert success
                Swal.fire({
                    icon: 'success',
                    title: 'Success!',
                    text: response.message,
                });
            } else {
                // SweetAlert error
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: response.message,
                });
            }
        },
        error: function () {
            // SweetAlert error for server-side issues
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong, please try again later.',
            });
        }
    });
    
        
})