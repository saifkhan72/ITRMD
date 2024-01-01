const updateSelect = element => {
    $( element ).selectize( {
        create: false,
        sortField: "text",
    } )
}
const getSelectItems = function ( element, apiUrl, id, pid ){    
    $( element ).html( `<option value="0">loading</option>` )
    let htmlRelation = ''
    $.ajax( apiUrl+'?id='+id+'&pid='+pid ).then( res => {
        htmlRelation +=`<option value="0">select one</option>`
        res.map( item => {
            if ( item.id == id ) {
                htmlRelation += `<option value='${ item.id }' selected='selected'>${ item.name }</option>`
            } else {

                htmlRelation += `<option value='${ item.id }'>${ item.name }</option>`
            }
        } )
        $( element ).selectize()[ 0 ].selectize.destroy();
        $( element ).html( htmlRelation )
        updateSelect( element )
    } )
   
}









