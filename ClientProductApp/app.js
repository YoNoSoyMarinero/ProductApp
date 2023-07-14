//DOM ELEMENTS
const REGISTRATION_FORM_DIV = document.getElementById("registration-form-div");
const LOGIN_FORM_DIV = document.getElementById("login-form-div");
const REGISTRATION_FORM = document.getElementById("registration-form");
const LOGIN_FORM = document.getElementById("login-form");
const APP_DIV = document.getElementById("app-div");
const LOGGED_USER_SPAN = document.getElementById("logged-user-span");
const LOGOUT_BUTTON = document.getElementById("logout-button");
const TABLE_DIV = document.getElementById("table-div");
const ADD_ELEMENT_FORM = document.getElementById("add-element-form");
const EDIT_ELEMENT_FORM = document.getElementById("edit-element-form");
const ADD_ELEMENT_DIV = document.getElementById("add-element-div");
const EDIT_ELEMENT_DIV = document.getElementById("edit-element-div");
const GIVEUP_POST_BUTTON = document.getElementById("giveup-post-button");
const GIVEUP_EDIT_BUTTON = document.getElementById("giveup-edit-button");
const PRICE_FILTER_INPUT = document.getElementById("price-filter-input");
const PRICE_FILTER_BUTTON = document.getElementById("price-filter-button");
const SELECT_FILTER_SELECT = document.getElementById("select-filter-select");
const FILTER_BY_SELECT_DIV = document.getElementById("filter-by-select-div");
const SELECT_FILTER_BUTTON = document.getElementById("select-filter-button")
const FILTER_BY_PRICE_DIV = document.getElementById("filter-by-price-div");
const LOGIN_FORM_REGISTRATION_BUTTONN = document.getElementById("login-form-registration-button");
const REGISTRATION_FORM_LOGIN_BUTTON = document.getElementById("registration-form-login-button");

//GLOBAL VARIABLES
let loggedIn = false;
let jwt = "";

//VALIDATION FUNCTION
const validateInput1 = (input) => {
    console.log(input.value.length)
    if(input.value.length < 2){
        return false;
    }
    return true;
}
const validateInput2 = (input) => {
    if(input.value.length < 2){
        return false;
    }
    return true;
}
const validateInput3 = (input) => {
    return true;
}

const validateSearch = (input) => {
    return true;
}


//HELPER FUNCTIONS
const getUrl = (endpoint) => {
    const PORT = "44395";
    const URL = `https://localhost:${PORT}/`;
    return URL + endpoint;
}

const createTable = (data) => {
    document.getElementById("table-div").innerHTML = "";
    const table = document.createElement("table");
    table.className = "table table-striped table-bordered table-hover";
    const headers = ["Name", "Category", "Price"];
  
    const headerRow = document.createElement("tr");
    headers.forEach((header) => {
      const th = document.createElement("th");
      th.textContent = header;
      headerRow.appendChild(th);
    });
    table.appendChild(headerRow);
  
    data.forEach((item) => {
      const row = document.createElement("tr");
      const cell1 = document.createElement("td");
      cell1.textContent = item.name;
      row.appendChild(cell1);
  
      const cell2 = document.createElement("td");
      cell2.textContent = item.category.name;
      row.appendChild(cell2);
  
      const cell3 = document.createElement("td");
      cell3.textContent = item.price;
      row.appendChild(cell3);

      const cell4 = document.createElement("td");
      cell4.textContent = item.activeSince;
      row.appendChild(cell4);

      if (loggedIn){
  
        const editButton = document.createElement("button");
        editButton.textContent = "Edit";
        editButton.className = "col-12 btn btn-info";
        editButton.setAttribute("data-id", item.id);
        editButton.addEventListener("click", handleEdit);
        row.appendChild(editButton);
    
        const deleteButton = document.createElement("button");
        deleteButton.textContent = "Delete";
        deleteButton.className = "col-12 btn btn-danger";
        deleteButton.setAttribute("data-id", item.id);
        deleteButton.addEventListener("click", handleDelete);
        row.appendChild(deleteButton);
      }
      table.appendChild(row);
    });
    document.getElementById("table-div").appendChild(table);
  };

const populateSelect = (data, select) => {
    while (select.firstChild) {
        select.removeChild(select.firstChild);
    }
    data.forEach((item) => {
      const option = document.createElement("option");
      option.value = item.id;
      option.textContent = item.name;
      select.appendChild(option);
    });
  };

//EVENT HANDLERS
const handleDOMLoad = () => {
    LOGIN_FORM_DIV.style.display = loggedIn ? "none" : "block";
    REGISTRATION_FORM_DIV.style.display = "none";
    ADD_ELEMENT_DIV.style.display = loggedIn ? "block" : "none";
    FILTER_BY_PRICE_DIV.style.display = loggedIn ? "block" : "none";
    FILTER_BY_SELECT_DIV.style.display = loggedIn ? "block": "none";
    EDIT_ELEMENT_DIV.style.display = "none";
    LOGOUT_BUTTON.style.display = loggedIn ? "block" : "none";
    loadData()
}

const handleShowRegistration = () => {
    LOGIN_FORM_DIV.style.display = "none";
    REGISTRATION_FORM_DIV.style.display = "block";
}

const handleLogut = () => {
    loggedIn = false;
    jwt = "";
    LOGGED_USER_SPAN.innerHTML = "";
    LOGOUT_BUTTON.style.display = "none";
    handleDOMLoad();
}

//ASYNC FUNCTIONS
async function loadData(){
    TABLE_DIV.innerHTML = "";
    const tableData = await getHttpRequest(getUrl("api/Product"));
    const selectData = await getHttpRequest(getUrl("api/Category"));
    const addSelect = document.getElementById("input3-post");
    const editSelect = document.getElementById("input3-edit");
    const appSelect = [addSelect, editSelect, SELECT_FILTER_SELECT]
    createTable(tableData);
    appSelect.forEach(s => populateSelect(selectData, s))
}

//ASYNC EVENT HANDLERS
async function handleSearch(){
    const data = await getHttpRequest(getUrl(`api/Product/searchByPrice/${PRICE_FILTER_INPUT.value}`));
    createTable(data);
}

async function handleSelectSearch(){
    console.log("Haha")
    const data = await getHttpRequest(getUrl(`api/Product/searchByCategory/${SELECT_FILTER_SELECT.options[SELECT_FILTER_SELECT.selectedIndex].text}`))
    createTable(data);
}

async function handleLogin(e){
    e.preventDefault();
    const usernameInput = document.getElementById("username-login")
    const passwordInput = document.getElementById("password-login")

    let user = {
        username : usernameInput.value,
        password : passwordInput.value
    }

    let data = await postHttpRequest(getUrl("api/Authentication/login"), user)
    jwt = data.token
    loggedIn = true
    LOGIN_FORM.reset(); 
    LOGGED_USER_SPAN.innerHTML = data.username;
    handleDOMLoad();
}

async function handleRegistration(e) {
    e.preventDefault();
    const usernameInput = document.getElementById("username-registration")
    const passwordInput = document.getElementById("password-registration")
    const passwordConfirmInput = document.getElementById("confirm-password-registration")
    const emailInput = document.getElementById("email-registration")

    const re = new RegExp(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^\w\s]).{6,}$/)
    if (!re.test(passwordInput.value)){
        alert("Password hast to have minimum six charachters, one uppercase letter, one lowercase letter and one special symbol");
        return;
    }

    if (passwordInput.value !== passwordConfirmInput.value){
        alert("Passwords must match")
        return;
    }
    if (usernameInput.length < 3){
        alert("Username has to be longer than 2 charachters")
        return;
    }
    let user = {
        username: usernameInput.value,
        password: passwordInput.value,
        email: emailInput.value
    }
    let data = await postHttpRequest(getUrl("api/Authentication/register"), user);
    REGISTRATION_FORM.reset();
    handleDOMLoad();
}

async function handlePost(e){
    e.preventDefault();
    const input1 = document.getElementById("input1-post")
    const input2 = document.getElementById("input2-post")
    const input3 = document.getElementById("input3-post")

    if (!validateInput1(input1)){
        alert("Input1 error message")
        return;
    }

    if (!validateInput2(input2)){
        alert("Input2 error message")
        return;
    }

    if (!validateInput3(input3)){
        alert("Input1 error message")
        return;
    }

    const product = {
        name: input1.value,
        price: input2.value,
        categoryId: input3.value
    }
    let data = await postHttpRequest("https://localhost:44395/api/Product", product)
    ADD_ELEMENT_FORM.reset()
    handleDOMLoad()
}

async function handleEdit(event){
    let id = event.target.getAttribute("data-id")
    EDIT_ELEMENT_DIV.style.display = "block"

    let data = await getHttpRequest(getUrl(`api/Product/${id}`))
    const input1 = document.getElementById("input1-edit")
    const input2 = document.getElementById("input2-edit")
    const input3 = document.getElementById("input3-edit")
    const idInput = document.getElementById("id-edit")

    idInput.value = data.id
    input1.value = data.name
    input2.value = data.price
    input3.value = data.categoryId
}

async function handleEditSubmit(e){
    e.preventDefault()
    const input1 = document.getElementById("input1-edit").value
    const input2 = document.getElementById("input2-edit").value
    const input3 = document.getElementById("input3-edit").value
    const id = document.getElementById("id-edit").value

    if (!validateInput1(input1)){
        alert("Input1 error message")
        return;
    }

    if (!validateInput2(input2)){
        alert("Input2 error message")
        return;
    }

    if (!validateInput3(input3)){
        alert("Input1 error message")
        return;
    }

    const product = {
        name: input1,
        price: parseFloat(input2),
        categoryId: parseInt(input3),
        id: parseInt(id)
    }

    let data = await putHttpRequest(getUrl(`api/Product/${id}`), product)
    EDIT_ELEMENT_FORM.reset()
    EDIT_ELEMENT_DIV.style.display = "none"
    handleDOMLoad()
}

async function handleDelete(event){
    let id = event.target.getAttribute("data-id")
    let data = await deleteHttpRequest(getUrl(`api/Product/${id}`))
    handleDOMLoad()
}


//HTTP REQUESTS FUNCTIONS
const getHttpRequest = (url) => {
    let headers = new Headers();
    headers.append("Content-type", "application/json")
    headers.append("Authorization", "Bearer " + jwt)

    return fetch(url, {
        method: "GET",
        headers: headers
    })
    .then((response) => {
        if (!response.ok) {
            throw new Error("Request failed with status: " + response.status);
        }
        return response.json();
    })
    .then(data => {
        return data;
    })
    .catch(e => {
        console.log("Error: ", e);
    });
}
const postHttpRequest = (url, body) => {
    let headers = new Headers();
    headers.append("Content-type", "application/json")
    headers.append("Authorization", "Bearer " + jwt)
    return fetch(url, {
        method: "POST",
        headers: headers,
        body: JSON.stringify(body)
    })
    .then((response) => {
        if (!response.ok) {
            if (response.status === 401){
                alert("Wrong password or username")
            }
            throw new Error("Request failed with status: " + response.status);
        }
        return response.json();
    })
    .then(data => {
        return data;
    })
    .catch(e => {
        console.log("Error: ", e);
    });
};
const putHttpRequest = (url, body) => {
    let headers = new Headers();
    headers.append("Content-type", "application/json")
    headers.append("Authorization", "Bearer " + jwt)
  
    return fetch(url, {
        method: "PUT",
        headers: headers,
        body: JSON.stringify(body)
    })
    .then((response) => {
        if (!response.ok) {
            if (response.status === 401){
                alert("Wrong password or username or you need to login before you do that")
            }
            throw new Error("Request failed with status: " + response.status);
        }
        return response.json();
    })
    .then(data => {
        return data;
    })
    .catch(e => {
        console.log("Error: ", e);
    });
}
const deleteHttpRequest = (url) => {
    let headers = new Headers();
    headers.append("Content-type", "application/json")
    headers.append("Authorization", "Bearer " + jwt)

    return fetch(url, {
        method: "DELETE",
        headers: headers
    })
    .then((response) => {
        if (!response.ok) {
            throw new Error("Request failed with status: " + response.status);
        }
        return response;
    })
    .catch(e => {
        console.log("Error: ", e);
    });
}


document.addEventListener("DOMContentLoaded", handleDOMLoad)
LOGIN_FORM.addEventListener("submit", handleLogin)
REGISTRATION_FORM.addEventListener("submit", handleRegistration)
ADD_ELEMENT_FORM.addEventListener("submit", handlePost)
EDIT_ELEMENT_FORM.addEventListener("submit", handleEditSubmit)
LOGOUT_BUTTON.addEventListener("click", handleLogut)
PRICE_FILTER_BUTTON.addEventListener("click", handleSearch)
LOGIN_FORM_REGISTRATION_BUTTONN.addEventListener("click", handleShowRegistration)
REGISTRATION_FORM_LOGIN_BUTTON.addEventListener("click", handleDOMLoad)
SELECT_FILTER_BUTTON.addEventListener("click", handleSelectSearch)
GIVEUP_POST_BUTTON.addEventListener("click", () => ADD_ELEMENT_FORM.reset())
GIVEUP_EDIT_BUTTON.addEventListener("click", () => EDIT_ELEMENT_FORM.reset())