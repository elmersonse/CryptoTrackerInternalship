fetch('/api/TransactionApi')
    .then(res => res.json())
    .then(res => {
        const grouped = groupBy(res);
        const keys = Object.keys(grouped).reverse();
        let list = keys.map(item => {
            let date = new Date(parseInt(item)).customFormat("#D# #MMMM# #YYYY#");
            return(
                <div>
                    <details className="card" open>
                        <summary className="card-header">{date}</summary>
                        {grouped[item].sort( (a, b) => b.id - a.id ).map(x => {
                            let image;
                            if(x.transactionType == "Get") {
                                image = <img src="../icons/cash-plus.png" alt="cash-plus"/>
                            }
                            if(x.transactionType == "Send") {
                                image = <img src="../icons/cash-minus.png" alt="cash-minus"/>
                            }
                            return(
                                <div>
                                    <div className="card-body">
                                        <div className="custom-card">
                                            <Link id={x.id} image={image} fullName={x.fullName} comm={x.commentary} amount={x.amount} curr={x.currency} transactionType={x.transactionType} commission={x.commission} wallet={x.wallet} />
                                        </div>
                                    </div>
                                </div>
                            );
                        })}
                    </details>
                </div>
            );
        });

        ReactDOM.render(
            <JsonData list={list}/>,
            document.getElementById("app")
        )
    });


function setId() {
    document.forms["modalForm"].elements["modal-id"].value = 0;
    document.forms["modalForm"].reset();
    document.getElementById("del").hidden = true;
}

async function createTransaction(curr, amount, commission, wallet, type, comment) {
    const resp = await fetch("/api/TransactionApi", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            currency: curr,
            amount: amount,
            commission: commission,
            wallet: wallet,
            transactionType: type,
            commentary: comment
        })
    });

    if(resp.ok === true) {
        location.reload();
    }
}

async function updateTransaction(curr, amount, commission, wallet, type, comment, id) {
    const resp = await fetch("/api/TransactionApi", {
        method: "PUT",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            id: id,
            currency: curr,
            amount: amount,
            commission: commission,
            wallet: wallet,
            transactionType: type,
            commentary: comment
        })
    });

    if(resp.ok === true) {
        location.reload();
    }
}

async function deleteTransaction(id) {
    const resp = await fetch("/api/TransactionApi/"+id, {
        method: "DELETE",
        headers: { "Accept": "application/json" },
    });

    if(resp.ok === true) {
        location.reload();
    }
}

class JsonData extends React.Component {
    constructor(props) {
        super(props);
        
    }

    render() {
        return (
            <div>
                {this.props.list}
                <Modal key={"modal"} />
            </div>
        );
    }
}

class Link extends React.Component {
    constructor(props) {
        super(props);

        this.handleClick = this.handleClick.bind(this);
    }

    handleClick() {
        let form = document.forms["modalForm"];
        document.getElementById("del").hidden = false;
        form.elements["modal-id"].value = this.props.id;
        form.elements["currency-select"].value = this.props.fullName;
        form.elements["amount"].value = this.props.amount;
        form.elements["commission"].value = this.props.commission;
        form.elements["wallet"].value = this.props.wallet;
        form.elements["type-select"].value = this.props.transactionType
        form.elements["commentary"].value = this.props.comm;
    }

    render() {
        return(
            <a className="custom-card-body" data-toggle="modal" href="#modal-1" onClick={this.handleClick}>
                <div className="custom-card-body-left">
                    <div className="custom-card-image">{this.props.image}</div>
                    <div className="custom-card-currency">{this.props.fullName}</div>
                    <div className="custom-card-commentary">{this.props.comm}</div>
                </div>
                <div className="custom-card-amount">{this.props.amount} {this.props.curr}</div>
            </a>
        );
    }
}

class Modal extends React.Component {
    constructor(props) {
        super(props);

        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleDelete = this.handleDelete.bind(this);
    }

    handleSubmit(e) {
        e.preventDefault();
        const form = document.forms["modalForm"];
        const id = form.elements["modal-id"].value;
        const curr = form.elements["currency-select"].selectedIndex.toString();
        const amount = form.elements["amount"].value;
        const commission = form.elements["commission"].value;
        const wallet = form.elements["wallet"].value;
        const type = form.elements["type-select"].selectedIndex.toString();
        const comm = form.elements["commentary"].value;

        if(id == 0) {
            createTransaction(curr, amount, commission, wallet, type, comm);
        }
        else{
            updateTransaction(curr, amount, commission, wallet, type, comm, id);
        }
    }

    handleDelete() {
        const id = document.forms["modalForm"].elements["modal-id"].value;
        deleteTransaction(id);
    }

    render() {
        return(
            <div className="modal" id="modal-1">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-body">
                            <button type="button" className="close modal-close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <br />
                            <div className="d-flex align-content-center justify-content-center">
                                <form name="modalForm" onSubmit={this.handleSubmit}>
                                    <input id="modal-id" type="text" value="0" hidden/>
                                    <div className="col modal-custom d-flex flex-column align-items-center">
                                        <div className="form-group col-md-8">
                                            <label className="col-form-label">Currency</label>
                                            <select id="currency-select">
                                                <option>Bitcoin</option>
                                                <option>Ethereum</option>
                                                <option>Tether</option>
                                                <option>Ripple</option>
                                                <option>BNB</option>
                                            </select>
                                        </div>
                                        <div className="form-group col-md-8">
                                            <label className="col-form-label">Amount</label>
                                            <input id="amount" type="number" step="any" required/>
                                        </div>
                                        <div className="form-group col-md-8">
                                            <label className="col-form-label">Commission</label>
                                            <input id="commission" type="number" step="1" min="0" value="0"></input>
                                        </div>
                                        <div className="form-group col-md-8">
                                            <label className="col-form-label">Wallet</label>
                                            <input id="wallet" type="text" />
                                        </div>
                                        <div className="form-group col-md-8">
                                            <label className="col-form-label">Transaction Type</label>
                                            <select id="type-select">
                                                <option>Get</option>
                                                <option>Send</option>
                                            </select>
                                        </div>
                                        <div className="form-group col-md-8">
                                            <label className="col-form-label">Commentary</label>
                                            <textarea id="commentary" type="text"></textarea>
                                        </div>
                                    </div>
                                    <div className="d-flex flex-row justify-content-around">
                                        <input id="subm" className="modal-save" type="image" src="../icons/check.png" alt="submit" />
                                        <a id="del" className="modal-delete" onClick={this.handleDelete}><img src="../icons/delete.png" alt="delete"/></a>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

