<><div class="well">

    <a href="#" class="btn btn-success" onclick="SendOTP()">Send OTP</a>

</div><script>
        var SendOTP = function () {$.ajax({
            url: "/Test/SendOTP",
            type: "post",
            success: function(data) {
                if (data == "success") {
                    alert("OTP sent successfully");
                    window.location = "/Test/EnterOTP";
                }
                else {
                    alert("Ask for another OTP");
                }
            }
        })}
    </script></>