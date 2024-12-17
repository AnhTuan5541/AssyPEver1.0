// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length === 2) return parts.pop().split(";").shift();
}
function deleteCookie(name) {
    document.cookie = name + "=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC;";
}
function getUserInfo(userid) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: '/Auth/GetUserInfo',
            type: 'POST',
            data: {
                "userid": userid
            },
            success: function (res) {
                const _resMail = res.data[0].email.toLowerCase();
                let us_in_token = `${res.data[0].id_card}|${res.data[0].display_name}|${_resMail}|${res.data[0].department}|${res.data[0].system_list}|${res.data[0].user_role}|${res.data[0].is_active}`;
                document.cookie = "us_in_token=" + us_in_token + "; path=/; expires=" + new Date(new Date().getTime() + 7 * 24 * 60 * 60 * 1000).toUTCString();
                resolve(res); // Resolve the promise with the response
            },
            error: function (err) {
                reject(err); // Reject the promise with the error
            }
        });
    });
}
function convertDate(strDate) {
    if (typeof (strDate) == 'object' && Object.keys(strDate).length === 0) {
        return ''
    }
    else {
        return strDate.substring(0, 10) + ' ' + strDate.substring(11, 19)
    }
}

function convertDate2(strDate) {
    if (typeof (strDate) == 'object' && Object.keys(strDate).length === 0) {
        return ''
    }
    else {
        return strDate.substring(0, 10)
    }
}

function setupPagination(data, isAdmin, isSign) {
    const rowsPerPage = 10;
    const pageCount = Math.ceil(data.length / rowsPerPage);
    let pagination = '';

    for (let i = 1; i <= pageCount; i++) {
        pagination += `<a href="javascript:void(0)" class="a-pagination " id="idPaginate_${i}" onclick="renderTable(dataApplication, ${i})">${i}</a>`;
    }

    $('#pagination').html(pagination);
    renderTable(data, 1, rowsPerPage, isAdmin, isSign);
}
function renderTable(data, page, rowsPerPage, isAdmin, isSign) {
    const start = (page - 1) * rowsPerPage;
    const end = start + rowsPerPage;
    const paginatedData = data.slice(start, end);

    let tableBody = '';
    paginatedData.forEach(row => {
        let classBadge = '';
        if (row.value_sign == 1) { classBadge = 'badge-warning' }
        if (row.value_sign == 2) { classBadge = 'badge-primary' }
        if (row.value_sign == 3) { classBadge = 'badge-success' }
        if (row.value_sign == 4) { classBadge = 'badge-danger' }
        if (row.value_sign == 5) { classBadge = 'badge-info' }
        if (row.value_sign == 7) { classBadge = 'badge-secondary' }

        let system = ''
        if (typeof (row.sub_system) == 'object' && Object.keys(row.sub_system).length === 0) {
            system = row.system
        } else {
            system = row.sub_system
        }

        let htmlHref = ''
        if(isAdmin && isSign) htmlHref = `/admin/detail?sign=1&sequence=${row.sequence}&no=${row.application_no}`
        if(isAdmin && !isSign) htmlHref = `/admin/detail?sign=0&sequence=${row.sequence}&no=${row.application_no}`
        if(!isAdmin && isSign) htmlHref = `/application/detail?sign=1&no=${row.application_no}`
        if(!isAdmin && !isSign) htmlHref = `/application/detail?sign=0&no=${row.application_no}`
            

        tableBody += `<tr>
                            <td><a href="javascript:void(0)" onclick="getSignFlow(${row.id})" data-toggle="modal" data-target="#modal-${row.application_no}">${row.application_no}</a></td>
                            <td>${truncateString(row.bussiness_justification)}</td>
                            <td>${row.name_user_require}</td>
                            <td>${system}</td>
                            <td>${row.display_name}</td>
                            <td><span class="badge ${classBadge}">${row.name_value_sign}</span></td>
                            <td><a href="${htmlHref}" type="button" class="btn btn-warning">Detail</a></td>
                        </tr>`;

        htmlModal = ` <div class="modal fade" id="modal-${row.application_no}">
                                        <div class="modal-dialog modal-xl">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                        <h4 class="modal-title">Application No: <b>${row.application_no}</b></h4>
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>
                                                </div>
                                                <div class="modal-body">
                                                   <table class="table table-bordered table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Singer</th>
                                                                <th>Sequence</th>
                                                                <th>Sign At</th>
                                                                <th>Comment</th>
                                                                <th>Start Date</th>
                                                                <th>Due Date</th>
                                                                <th>Status</th>
                                                                <th>File Attach</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tbody-modal-${row.id}"></tbody>
                                                   </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>`;
        $('#modal-all').append(htmlModal)

    });

    $('#tBody').html(tableBody);

    const menuItems = document.querySelectorAll('.a-pagination');
    menuItems.forEach(item => {
        item.addEventListener('click', () => {
            // Loại bỏ lớp 'active' khỏi tất cả các menu
            menuItems.forEach(menu => {
                menu.classList.remove('active');
            });

            // Thêm lớp 'active' cho menu được click
            item.classList.add('active');
        });
    });
    if (page == 1) {
        $('#idPaginate_' + page).addClass("active");
    }
}

function getSignFlow(idApp) {
    $.ajax({
        url: '/Application/GetSignFlowByIdApplication',
        type: 'GET',
        headers: {
            'Authorization': 'Bearer ' + getCookie('auth_token')
        },
        data: { idApp: idApp },
        success: function (res) {
            let data = res.data
            let htmlTBodyModal = ''
            for (let i = 0; i < data.length; i++) {
                let classBadge = '';
                if (data[i].id_value_sign == 1) { classBadge = 'badge-warning' }
                if (data[i].id_value_sign == 2) { classBadge = 'badge-primary' }
                if (data[i].id_value_sign == 3) { classBadge = 'badge-success' }
                if (data[i].id_value_sign == 4) { classBadge = 'badge-danger' }
                if (data[i].id_value_sign == 5) { classBadge = 'badge-info' }
                if (data[i].id_value_sign == 6) { classBadge = 'badge-secondary' }
                if (data[i].id_value_sign == 7) { classBadge = 'badge-secondary' }

                let htmlFile = '';
                if (data[i].file) { 
                    let urlList = data[i].file_name.split('.')
                    let extensionFile = urlList[urlList.length - 1]

                    let base64 = 'data:text/plain;base64,' + data[i].file;

                    let baseUrl = `${window.location.protocol}//${window.location.hostname}:${window.location.port}`;
                    
                    if (extensionFile === 'docx') {
                        htmlFile = `<a href="${base64}" download="${data[i].file_name}"><img src="${baseUrl}/lib/dist/img/docx.png" alt="Docx" class="file-image" /> ${data[i].file_name}</a>`
                    }
                    if (extensionFile === 'pdf') {
                        htmlFile = `<a href="${base64}" download="${data[i].file_name}"><img src="${baseUrl}/lib/dist/img/pdf.jpg" alt="PDF" class="file-image" /> ${data[i].file_name}</a>`
                    }
                    if (extensionFile === 'xlsx' || extensionFile === 'xls') {
                        htmlFile = `<a href="${base64}" download="${data[i].file_name}"><img src="${baseUrl}/lib/dist/img/excel.png" alt="Excel" class="file-image" /> ${data[i].file_name}</a>`
                    }
                    if (extensionFile === 'pptx') {
                        htmlFile = `<a href="${base64}" download="${data[i].file_name}"><img src="${baseUrl}/lib/dist/img/powerpoint.png" alt="Powerpoint" class="file-image" /> ${data[i].file_name}</a>`
                    }
                }
                htmlTBodyModal += `<tr>
                                        <th>${data[i].display_name}</th>
                                        <th>${data[i].sequence}</th>
                                        <th>${convertDate(data[i].sign_at)}</th>
                                        <th>${data[i].comment}</th>
                                        <th>${convertDate2(data[i].start_date)}</th>
                                        <th>${convertDate2(data[i].end_date)}</th>
                                        <th><span class="badge ${classBadge}">${data[i].value_sign}</span></th>
                                        <th>${htmlFile}</th>
                                    </tr>`

            }
            $('#tbody-modal-' + idApp).html(htmlTBodyModal)

        },
        error: function (err) {
            if (err.status == 401) {
                logout();
            }
            else {
                alert(err);
            }
        }
    });
}
function truncateString(str) {
    let maxLength = 100;
    if (str.length > maxLength) {
        return str.substring(0, maxLength) + '...';
    }
    return str;
}