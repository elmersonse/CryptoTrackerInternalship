class LayoutNoAuth extends React.Component {
    constructor() {
        super();
    }

    render() {
        return (
            <header>
                <nav className="navbar navbar-expand-sm navbar-toggleable-sm box-shadow mb-3">
                    <div className="container">
                        <a className="navbar-brand nav-text-light" asp-area="" href="http://localhost:5001/">CryptoTracker</a>
                        <button className="navbar-toggler" type="button" data-toggle="collapse"
                                data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                                aria-expanded="false" aria-label="Toggle navigation">
                            <span className="navbar-toggler-icon"></span>
                        </button>
                        <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                            <ul className="navbar-nav flex-grow-1 justify-content-between">
                                <div className="d-flex flex-row">
                                    <li className="nav-item">
                                        <a className="nav-link nav-text-light" href="http://localhost:5001/">Home</a>
                                    </li>
                                </div>
                                <div className="d-flex flex-row">
                                    <li className="nav-item">
                                        <a className="nav-link nav-text-light" href={"http://localhost:5001/account/login/"}>Login</a>
                                    </li>
                                    <li className="nav-item">
                                        <a className="nav-link nav-text-light" href={"http://localhost:5001/account/register/"}>Register</a>
                                    </li>
                                </div>
                            </ul>
                        </div>
                    </div>
                </nav>
            </header>
        );
    }
}

ReactDOM.render(
    <LayoutNoAuth />,
    document.getElementById("noAuth")
);