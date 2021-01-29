// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var rate = 0;
var pageNum = 1;
$("#subscribe").keypress(function (event) {
    if (event.which == 13) {
        var email = $("#subscribe").val();
        if (isEmail(email)) {
            $.post("/Home/Subscribe/", { email: email }, function (data) {
                if (data === 1) {
                    $("#subscribe").val('');
                    alert('Thanks for subscribing our newsletter...');
                }
                else {
                    alert('Email Already Exists...');
                }
            }
            );
        }
        else {
            alert(email+' is not a valid email id...');
        }
    }
});
function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}
function rateRecipe(counter) {
    rate = counter;
    for (var i = 1; i <= 5; i++) {
        if (i <= counter) {
            $("#rat" + i).addClass("fa-star");
            $("#rat" + i).removeClass("fa-star-o");
        }
        else {
            $("#rat" + i).addClass("fa-star-o");
            $("#rat" + i).removeClass("fa-star");
        }
    }
}

function likeRecipe(id) {
    $("#like").addClass("fa-thumbs-up");
    $("#like").removeClass("fa-thumbs-o-up");
    $.post("/Recipe/AddLike", { id: id }, function (data) {
        $("#likes").html(data);
    }); 
}
$('#reviewta').on('keyup', function (event) {
    var len = $(this).val().length;
    if (len >= 500) {
        $(this).val($(this).val().substring(0, len - 1));
        $('#leftChars').text('500 of 500');

    }
    else {
        $('#leftChars').text(len + ' of 500');
    }

});

function paginate(element,pagesize) {
    if (element.innerText === 'Next' || element.innerText === '>') {
        if (pageNum != $('#hdnTotalPages').val()) {
            pageNum = pageNum + 1;
            var testimonials = $('.page-link');
            testimonials.removeClass('active');
            var lastRecord = pageNum * pagesize;
            var intialRecord = lastRecord - pagesize+1;
            for (var i = 1; i <= $('#hdnTotalCount').val(); i++) {
                $('#page_' + i).css('display', 'none');
                if (i >= intialRecord && i <= lastRecord) {
                    $('#page_' + i).css('display', '');
                }
            }
            for (var i = 0; i < testimonials.length; i++) {
                // Using $() to re-wrap the element.
                if (testimonials[i].innerText == pageNum) {
                    testimonials[i].classList.add('active');
                }

            }

        }
    }
    else if (element.innerText === 'Previous' || element.innerText === '<')
    {
        if (pageNum != 1) {
            pageNum = pageNum - 1;
            var testimonials = $('.page-link');
            testimonials.removeClass('active');
            var lastRecord = pageNum * pagesize;
            var intialRecord = lastRecord - pagesize+1;
            for (var i = 1; i <= $('#hdnTotalCount').val(); i++) {
                $('#page_' + i).css('display', 'none');
                if (i >= intialRecord && i <= lastRecord) {
                    $('#page_' + i).css('display', '');
                }
            }
            for (var i = 0; i < testimonials.length; i++) {
                // Using $() to re-wrap the element.
                if (testimonials[i].innerText == pageNum) {
                    testimonials[i].classList.add('active');
                }

            }
        }
    }
    else {
        var testimonials = $('.page-link');
        testimonials.removeClass('active');
        for (var i = 0; i < testimonials.length; i++) {
            // Using $() to re-wrap the element.
            if (testimonials[i].innerText === element.innerText) {
                testimonials[i].classList.add('active');
                pageNum = i;
            }

        }
        var lastRecord = element.innerText * pagesize;
        var intialRecord = lastRecord - pagesize+1;
        for (var i = 1; i <= $('#hdnTotalCount').val(); i++) {
            $('#page_' + i).css('display', 'none');
            if (i >= intialRecord && i <= lastRecord) {
                $('#page_' + i).css('display', '');
            }
        }
    }
    if (pageNum > 1 && pageNum < $('.page-link').length - 2) {
        var testimonials = $('.page-link');
        for (var i = 1; i <= $('.page-link').length - 2;i++) {
            testimonials[i].style.display = 'none'
        }
        testimonials[pageNum].style.display = '';
        testimonials[pageNum + 1].style.display = '';
        testimonials[pageNum - 1].style.display = '';

    }
}

function addReview(id) {
    var msg = '';
    if (rate === 0)
        msg = 'Please give stars to review... \n';
    var name = $('#ReviewerName').val();
    if (name === '')
        msg += 'Please Enter your name... \n';
    var reviewTitle = $('#ReviewTitle').val();
    if (reviewTitle === '')
        msg += 'Please Enter Review Title... \n';
    var review = $('#reviewta').val();
    if (review === '')
        msg += 'Please Enter Review... \n';
    if (msg != '') {
        alert(msg);
    }
    else{
        $.post('/Recipe/AddReview/', { id: id, reviewerName: name, reviewTitle: reviewTitle, review: review, rating: rate }, function (data) {
            if (data === 'Success') {
                var newElement = '<div class="row">           <div class="col-sm-1">               <span>' + rate + '</span>               <i class="fa fa-star" aria-hidden="true"></i>           </div>           <div class="col-sm-8 reviewerName">' + name + ' says</div>           <div class="col-sm-3 float-right">' + new Date().toDateString() + '</div>       </div>   <div class="row" style="    padding-left: 10%;    font-weight: 500;">'+reviewTitle+'</div>    <div class="row">           <div class="reviews">' + review + '</div>       </div>';
                $('#reviewComments').append(newElement);

            }
        });
        for (var i = 1; i <= 5; i++) {
                $("#rat" + i).addClass("fa-star-o");
                $("#rat" + i).removeClass("fa-star");            
        }
        $('#ReviewerName').val('');
        $('#ReviewTitle').val('');
        $('#reviewta').val('');
    }
}
function resetSearch() {
    $('#SubCatId').prop('selectedIndex', 0);
    $('#CookingTime').prop('selectedIndex', 0);
    $('#Difficulty').prop('selectedIndex', 0);
    $('#CookingMethod').prop('selectedIndex', 0);
    $('#RecipeName').val('');
    $('#Tag').val('');

}

$("#recipeSearch").keypress(function (event) {
    if (event.which == 13) {
        window.location.href = "Recipe/Index/?recipeName=" + $('#recipeSearch').val();
    }
});
$("#categorySearch").keypress(function (event) {
    if (event.which == 13) {
        window.location.href = "Category/Search/?category=" + $('#categorySearch').val();
    }
});

function OpenModal(element) {
    $('#imgPreview').attr('src', $('#galleryImg_'+element).attr('src'));
    $('#caption')[0].innerText = $('#galleryTitle_' + element)[0].innerText;
    $('#myModal').show();
    $('#hdnPreview').val(element);
}
$(".close").click(function () {
    $('#myModal').css('display', 'none');
});
$(".fa-chevron-circle-left").click(function () {
    var total = $('[name^="galleryImg"]').length - 1;
    var current = $('#hdnPreview').val();
    if (current == 0) {
        OpenModal(total);
    }
    else {
        OpenModal(Number(current)-1);
    }
});

$(".fa-chevron-circle-right").click(function () {
    var total = $('[name^="galleryImg"]').length - 1;
    var current = $('#hdnPreview').val();
    if (current == total) {
        OpenModal(0);
    }
    else {
        OpenModal(Number(current) + 1);
    }
});

function myFunction() {
    var input, filter, ul, li, a, i, txtValue;
    input = document.getElementById("myInput");
    filter = input.value.toUpperCase();
    ul = document.getElementById("myUL");
    li = ul.getElementsByTagName("li");
    var maxLength = 6;
    var counter = 0;
    if (filter === "") {
        for (i = 0; i < li.length; i++) {
            li[i].style.display = "none";
        }
    }
    else {
        for (i = 0; i < li.length; i++) {
            a = li[i].getElementsByTagName("a")[0];
            txtValue = a.textContent || a.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1 && counter <= maxLength) {
                li[i].style.display = "";
                counter = counter + 1;
            } else {
                li[i].style.display = "none";
            }
        }
    }
}

function searchIng() {
    var inputValue = document.getElementById("IngSearch").value.toUpperCase();
    var li = document.getElementById("Ings").getElementsByTagName("li");
    if (inputValue === "") {
        for (i = 0; i < li.length; i++) {
            li[i].style.display = "none";
        }
    }
    else {
        for (var i = 0; i < li.length; i++) {
            if (li[i].innerText.toUpperCase().indexOf(inputValue) > -1) {
                li[i].style.display = "";
            }
            else {
                li[i].style.display = "none";
            }
        }
    }
}

$(document).ready(function () {
    $(window).scroll(function () { // check if scroll event happened
        if ($(document).scrollTop() > 500) { // check if user scrolled more than 50 from top of the browser window
            $("#myNav").css("background-color", "yellowgreen"); // if yes, then change the color of class "navbar-fixed-top" to white (#f8f8f8)
        } else {
            $("#myNav").css("background-color", "transparent"); // if not, change it back to transparent
        }
    });
    $.get("/Recipe/GetDishes", function (data) {
        for (var i = 0; i < data.length; i++) {
            var newElement = '<li style="display:none;"><a href="/Recipe/Recipe/' + data[i].val+'">' + data[i].label+'</a></li>';
            $('#myUL').append(newElement);
        }
    }); 
});