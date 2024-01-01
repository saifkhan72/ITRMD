
let pageNumber = 0
let orderBy = 'id'
let direction = 'desc'
let search = 'null'
let pageLength = 10
let datatable = null
let thKeysG = []
let urlG = ''
let tableG=''
let id=0
let thG = null



$(document).ready(()=>{
    let datatable = $( "#datatable" )
    let table = document.getElementById( "tableNameId" ).value
    let url = datatable.data('url')
    urlG = url
    let th = $('thead').find('th')
    thG = th
    let thKeys = []
    th.map(item=>{
        thKeys.push($(th[item]).data('key'))
    })
    thKeysG = thKeys
    tableG = table;

     
    fetchData(tableG,url, pageNumber, pageLength, orderBy, direction, search, thKeys)

})

const resolve = (path, obj) => {
    return path.split('.').reduce(function (prev, curr) {
        return prev ? prev[curr] : null
    }, obj || self)
}

const downIcon = () => {
    return `<svg xmlns="http://www.w3.org/2000/svg" className="h-3 w-3" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                <path strokeLinecap="round" strokeLinejoin="round" d="M19 9l-7 7-7-7" />
            </svg>`
}

const refreshTable = () => {
    fetchData(tableGG,urlG, pageNumber, pageLength, orderBy,direction, search, thKeysG)
}

const upIcon = () => {
    return `<svg xmlns="http://www.w3.org/2000/svg" className="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                <path strokeLinecap="round" strokeLinejoin="round" d="M5 15l7-7 7 7" />
            </svg>`
}

const renderInputs = (item, res) => {

    switch (item?.input_type){
        case 'date':
            return `<div>
                        <input id="${item?.label}" type="date" name="${item?.label}" value="${resolve(item?.label, res)}">
                    </div>`
            break;

        case 'number':
            return `<div>
                        <input id="${item?.label}" type="number" name="${item?.label}" value="${resolve(item?.label, res)}">
                    </div>`
            break;
        case 'hidden':
            return `<div>
                        <input id="${item?.label}" type="hidden" name="${item?.label}" value="${resolve(item?.label, res)}">
                    </div>`
            break;
        case 'text':
            return `<div class="col s12 input-field">
                        <label for="${item?.label}">${item?.label}</label>
                        <input id="${item?.label}" type="text" name="${item?.label}" value="${resolve(item?.label, res)}">
                    </div>`
            break;
        case 'select':

            let url = item?.select_url

            let selectCheckValue = resolve(item?.select_id, res)

            let options = null
            $.ajax({
                url: url,
                success: selectRes=>{
                    let res_html = `
                        <label for="${item?.label}">${item?.label.split('.')[0]}</label>
                        <select name="${item?.label.split('.')[0]}" id="${item?.label}" class="browser-default">
                          ${selectRes?.map(item=>{
                                return `<option ${selectCheckValue==item?.id?'selected':''} value="${item?.id}">${item?.title}</option>`
                          })}
                        </select>
                    `
                    $(`#select_id_${item?.label.replaceAll('.','_')}`).html(res_html)

                }
            })
            return `<div class="col s12" id="select_id_${item?.label.replaceAll('.','_')}">
                        <p class="text-center">Loading...</p>
                    </div>`
    }
}

const deleteItem = id => {
    if (confirm('Are you sure')){
        $("#fullPageLoading").show()
        $.ajax({
            url: urlG+'/'+id,
            method: 'delete',
            success: ()=>{
                $("#fullPageLoading").hide()
                refreshTable()
                M.toast({html: 'Deleted successfully', classes: 'bg-green-500 rounded-xl'})
            },
            error: () => {
                $("#fullPageLoading").hide()
                M.toast({html: 'Something went wrong', classes: 'bg-red-500 rounded-xl'})
            }
        })
    }
}

const fetchData = ( table, url, pageNumber, pageLength, sort, direction, search, thKeys ) => { 
    setLoadingStatus()
    $.ajax({
        url: `${url}?pageNumber=${pageNumber}&pageLength=${pageLength}&orderBy=${sort}&direction=${direction}&search=${search}`,
        success: res=>{
            if(res?.data?.length < 1){
                $("#tbody").html(`
                    <tr>
                        <td colspan="1000" class="text-center text-red-500">No record found</td>
                    </tr>
                `)
            }else{
                let html = null
                res?.data?.map(item=>{
                    html+=`<tr>${thKeys?.map(key=>{return `
                        <td ${key == 'balance' ? 'class="balance"' : 'class=""' }> ${ resolve( key, item ) }</td>`
                    } ) }`
                    if ( table == 'leave' ) {
                        html += `<td><button onclick="aem.modal(event,'AllHRMF13DepartmentCheckByFormIdHr?id=${ item.id }')" class="anchorDetail mr-1" data-id="${ item.id }"><i class='material-icons'>info</i></button></td>`
                    }
                    if ( table == 'question' ) {
                        html += `<td><button onclick="aem.modal(event,'QuestionDetail?id=${ item.id }')" class="anchorDetail mr-1" data-id="${ item.id }"><i class='material-icons'>info</i></button></td>`
                        html += `<td><a target="_blank" href="Change/${ item.id }"><i class='material-icons'>edit</i></a><a target="_blank" href="Details/${ item.id}"><i class='material-icons'>format_color_text</i></a></td>`

                    }
                    html += `</tr >`
                })
                $('#tbody').html(html)
                let totalPages = res?.totalPages
                let pagniationItem = `
                             <li>
                                <span onclick="changePageNumber('1')" class='p-1 border cursor-pointer hover:bg-gray-400 ${1==pageNumber?'bg-gray-300':''}'> << </span>
                            </li>  
                `
                for(let i=1; i<=totalPages+1; i++){

                    if(i >= parseInt(pageNumber) && i<= (parseInt(pageNumber)+10)){
                        pagniationItem+=`
                            <li>
                                <span onclick="changePageNumber('${i}')" class='p-1 border cursor-pointer hover:bg-gray-400 ${i==pageNumber?'bg-gray-300':''}'>${i}</span>
                            </li>   
                            `
                    }else if(totalPages < parseInt(pageNumber)+10 && i>parseInt(pageNumber)-10){
                        pagniationItem+=`
                            <li>
                                <span onclick="changePageNumber('${i}')" class='p-1 border cursor-pointer hover:bg-gray-400 ${i==pageNumber?'bg-gray-300':''}'>${i}</span>
                            </li>   
                            `
                    }
                }
                pagniationItem += `
                <li>
                   <span onclick="changePageNumber('${totalPages-1}')" class='p-1 border cursor-pointer hover:bg-gray-400 ${totalPages-1==pageNumber?'bg-gray-300':''}'> >> </span>
               </li>

                <li style='text-align:left !important'>
                   <span>total record found:${res?.count} </span>
               </li>
   `
                $("#datatable-paination").html(pagniationItem)

                $(".balance")
                    .filter(function () {
                        return $(this).html() < 0;
                    })
                    .parent().css('background-color', '#ffcccb');
            }
        }
    })
}

const changePageLimit = event => {
    let newPageLength = $(event.target).val()
    pageLength = newPageLength
    console.log(newPageLength)
    fetchData(tableG,urlG, pageNumber, newPageLength, orderBy,direction, search, thKeysG)
}

const handelForm = event => {
    event.preventDefault()
    var modal = M.Modal.getInstance($('#modal1'));
    modal.open()
    const form_data = $(event.target).serializeArray()
    let form_btn = $(event.target).find('button[type="submit"]')
    let form_btn_html = form_btn.html()
    form_btn.html('Loading...').prop('disabled', true)
    let formData = new FormData()
    for (let key in form_data){
        formData.append(form_data[key]?.name, form_data[key]?.value)
    }

    $.ajax({
        url: urlG,
        data: formData,
        processData: false,
        cache: false,
        contentType: false,
        method: 'POST',
        success: function (res){
            if(res.result){
                M.toast({html: res?.message, classes: 'bg-green-500 rounded-xl'})
                form_btn.html(form_btn_html).removeAttr('disabled')
            }
            modal.close()
            fetchData(tableG,urlG, pageNumber, pageLength, orderBy,direction, search, thKeysG)
        },
        error: ()=>{
            form_btn.html(form_btn_html).removeAttr('disabled')
            M.toast({html: 'Something went wrong', classes: 'bg-red-500 rounded-xl'})
        }
    })
}

const closeModal = () => {
    var modal = M.Modal.getInstance($('#modal1'));
    modal.close()
}

const editForm= (table, id)=>{
    let html2 =
    $.ajax({
        url: `${urlG}/${id}`,
        success: res=> {
            let field = $(event.target).res?.('key')
            let heads = []
            thG.map(item=>{
                heads.push({
                    label: $(thG[item]).data('key'),
                    input_type:
                                typeof
                                    $(thG[item]).data('input-type') === 'undefined'? 'text' :
                                    $(thG[item]).data('input-type'),
                                            select_url: $(thG[item]).data('select-url'),
                                            select_id: $(thG[item]).data('select-id')
                })
            })
            let html2 = `
                <form onsubmit="handelForm(event)">
                    <div class="row">                 
                          ${heads?.map(item=>{
                                return renderInputs(item, res)}).join('')
               
                            }
                            <div class="col s12">
                            <br><br>
                                <div class="flex items-center justify-between">
                                    <button type="submit" class="btn btn-small waves-effect transparent black-text">
                                        Save <i class="material-icons right">save</i>
                                    </button>
                                    <button type="button" onclick="closeModal()" class="btn btn-small waves-effect transparent black-text">
                                        Cancel <i class="material-icons right">close</i>
                                    </button>
                                </div>
                            </div>
                      </div>
                </form>
            `
            var modal = M.Modal.getInstance($('#modal1'));
            modal.open()
            $('#modal-content1').html(html2)
            modal.open()
            M.updateTextFields();
        }

    })


}

const changePageNumber = (page) => {
    pageNumber = page
    fetchData(tableG,urlG, pageNumber, pageLength, orderBy, direction, search, thKeysG)
}

const changeSearch = event => {
    search = event;
    fetchData(tableG,urlG, pageNumber, pageLength, orderBy, direction, search, thKeysG)
}

const changeSort = event => {
    let field = $(event.target).data('key')
    orderBy = field

    if (field.includes('.')){
        orderBy = field.split('.')[0]
    }

    if(direction=='asc'){
        direction='desc';
    }else{
        direction='asc';
    }

    $('.icon-indicator').remove()

    $(event.target).html(
        `
            <div class='flex items-center space-x-2' ondblclick="changeSort(event)" data-key='${field}'>
                <div>
                    ${field}
                </div>
                <div class='icon-indicator'>
                    ${direction=='asc'?'<i class="material-icons tiny">expand_more</i>':'<i class="material-icons tiny">expand_less</i>'}
                </div>
            </div>
        `
    )
    fetchData(tableG,urlG, pageNumber, pageLength, orderBy, direction, search, thKeysG)
}

const setLoadingStatus = () => {
    let htmlbody = null
    for(var i =0;i<=10;i++){
        htmlbody+=`<tr><td colspan="100" class="text-center">Loading...</td></tr>`
    }

    let html = $('#tbody').html(htmlbody)
}