async function Login(login, password) {
    let err = document.getElementById("errorLabel");
    const resp = await fetch("/api/LoginApi", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            name: login,
            password: password
        })
    })
        .then(res => res.json())
        .then(res => {
            if(res === "Done") {
                err.hidden = true;
                location.assign(".");
                return;
            }
            err.innerHTML = res;
            err.hidden = false;
        });
    
}

class LoginForm extends React.Component {
    constructor() {
        super();
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    
    handleSubmit(e) {
        e.preventDefault();
        let form = document.forms["loginForm"];
        const login = form.elements["login"].value;
        const password = form.elements["password"].value;
        Login(login, password);
    }

    render() {
        return (
            <div className="d-flex align-items-center justify-content-center">
                <form name="loginForm" className="d-flex flex-column justify-content-center align-items-center" onSubmit={this.handleSubmit}>
                    <label id="errorLabel" className="errorLabel" hidden></label>
                    <input id="login" className="form-control" placeholder="Login" type="text" required/>
                    <input id="password" className="form-control" placeholder="Password" type="password" required/>
                    <button className="btn-success" type="submit">Login</button>
                </form>
            </div>
        );
    }
}

ReactDOM.render(
    <LoginForm />,
    document.getElementById("log")
);