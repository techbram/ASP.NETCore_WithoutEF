
function addToCart() {
    // Serialize form data manually
    var formData = $("#saveCartForm").serialize();
   
    $.ajax({
        type: "POST",
        url: '/Customer/Home/Detail', // Direct URL or use Razor variable if in .cshtml
        data: formData,
        success: function (response) {
            if (response.redirect) {
                window.location.href = response.redirectUrl;
            } else if (response.success) {
                // Update cart count
                if (response.cartCount !== undefined) {
                    $("#cart-count").text(response.cartCount);
                }
                Swal.fire({
                    icon: 'success',
                    title: 'Success!',
                    text: response.message,
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: response.message,
                });
            }
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong, please try again later.',
            });
        }
    });
}
//if (response.success) {
//    // Update cart count
//    if (response.cartCount !== undefined) {
//        $("#cart-count").text(response.cartCount);
//    }

//    Swal.fire({
//        icon: 'success',
//        title: 'Success!',
//        text: response.message
//    });
//}