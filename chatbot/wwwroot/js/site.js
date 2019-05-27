$(function () {
    'use strict'

    var navHeight = $('.navbar').outerHeight()
    $('body').attr('style', 'padding-top:' + navHeight + 'px !important')
    $('.offcanvas-collapse').attr('style', 'top: ' + navHeight + 'px !important')

    $('[data-toggle="offcanvas"]').on('click', function () {
        $('.offcanvas-collapse').toggleClass('open')
    })

})
