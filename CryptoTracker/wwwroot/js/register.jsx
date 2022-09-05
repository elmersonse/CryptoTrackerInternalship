async function Register(login, email, password, passwordConfirm) {
    let err = document.getElementById("errorLabel");
    await fetch("/api/RegisterApi", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            name: login,
            email: email,
            password: password,
            passwordConfirm: passwordConfirm
        })
    })
        .then(res => res.json())
        .then(res => {
            if(res === "Done") {
                err.hidden = true;
                location.assign("/");
                return;
            }
            if(res === "Check email for confirmation link") {
                err.className = "doneLabel";
            }
            else {
                err.className = "errorLabel";
            }
            err.innerHTML = res;
            err.hidden = false;
        });
}

class RegisterForm extends React.Component {
    constructor() {
        super();
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(e) {
        e.preventDefault();
        let form = document.forms["registerForm"];
        const login = form.elements["login"].value;
        const email = form.elements["email"].value;
        const password = form.elements["password"].value;
        const passwordConfirm = form.elements["passwordConfirm"].value;
        if(password.length < 8) {
            document.getElementById("errorLabel").innerHTML = "Password must be 8 at least digits";
        }
        Register(login, email, password, passwordConfirm);
    }

    render() {
        return (
            <div className="d-flex align-items-center justify-content-center">
                <form name="registerForm" className="d-flex flex-column justify-content-center align-items-center" onSubmit={this.handleSubmit}>
                    <label id="errorLabel" className="errorLabel" hidden></label>
                    <input id="login" className="form-control" placeholder="Login" type="text" required/>
                    <input id="email" className="form-control" placeholder="Email" type="email" required/>
                    <input id="password" className="form-control" placeholder="Password" type="password" required/>
                    <input id="passwordConfirm" className="form-control" placeholder="Confirm password" type="password" required/>
                    <button className="btn-success" type="submit">Register</button>
                </form>
            </div>
        );
    }
}

ReactDOM.render(
    <RegisterForm />,
    document.getElementById("reg")
);