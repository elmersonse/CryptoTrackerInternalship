fetch('./api/DealApi')
    .then(res => res.json())
    .then(res => {
        
        ReactDOM.render(
            <JsonData list={res} />,
            document.getElementById("app")
        )
    });


let rates = [];
fetch('https://api.coingecko.com/api/v3/simple/price?ids=bitcoin%2Cethereum%2Ctether%2Cripple%2Cbinancecoin&vs_currencies=usd')
    .then(res => res.json())
    .then(res => rates = [Math.round(res.bitcoin.usd * 100000) / 100000, 
        Math.round(res.ethereum.usd * 100000) / 100000,
        Math.round(res.tether.usd * 100000) / 100000,
        Math.round(res.ripple.usd * 100000) / 100000,
        Math.round(res.binancecoin.usd * 100000) / 100000
    ]);

function drawList(list) {
    const grouped = groupBy(list);
    const keys = Object.keys(grouped).reverse();
    let res = keys.map(key => {
        let date = new Date(parseInt(key)).customFormat("#D# #MMMM# #YYYY#");
        return(
            <details key={date} className="card" open>
                <summary className="card-header">{date}</summary>
                {grouped[key].sort( (a, b) => b.id - a.id ).map(x => {
                    let image;
                    if(x.dealType == "Buy") {
                        image = <img src="../icons/cash-plus.png" alt="cash-plus"/>
                    }
                    if(x.dealType == "Sell") {
                        image = <img src="../icons/cash-minus.png" alt="cash-minus"/>
                    }
                    if(x.dealType == "ShortBuy") {
                        image = <img src="../icons/cash-fast.png" alt="cash-fast"/>
                    }
                    return(
                        <div>
                            <div className="card-body">
                                <div className="custom-card">
                                    <Link id={x.id} image={image} fullName={x.fullName} comm={x.commentary} amount={x.amount} curr={x.currency} dealType={x.dealType} rate={x.rate} />
                                </div>
                            </div>
                        </div>
                    );
                })}
            </details>
        );
    });
    return res;
}

function setId() {
    document.forms["modalForm"].elements["modal-id"].value = 0;
    document.forms["modalForm"].reset();
    document.getElementById("del").hidden = true;
    const currency = document.getElementById("currency-select");
    const rate = document.getElementById("rate");
    const cb = document.getElementById("useLiveRate");
    cb.checked = true;
    rate.readOnly = true;
    rate.value = rates[currency.selectedIndex];
}

async function createDeal(curr, amount, rate, type, comment) {
    const resp = await fetch("./api/DealApi", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" }, 
        body: JSON.stringify({
            currency: curr,
            amount: amount,
            rate: rate,
            dealType: type,
            commentary: comment
        })
    });
    
    if(resp.ok === true) {
        location.reload();
    }
}

async function updateDeal(curr, amount, rate, type, comment, id) {
    const resp = await fetch("./api/DealApi", {
        method: "PUT",
        headers: { "Accept": "application/json", "Content-Type": "application/json" }, 
        body: JSON.stringify({
            id: id,
            currency: curr,
            amount: amount,
            rate: rate,
            dealType: type,
            commentary: comment
        })
    });
    
    if(resp.ok === true) {
        location.reload();
    }
}

async function deleteDeal(id) {
    const resp = await fetch("./api/DealApi/"+id, {
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
        this.state = {list:this.props.list};
        this.showCurr = this.showCurr.bind(this);
        this.showDealType = this.showDealType.bind(this);
    }
    
    showCurr(curr) {
        if(curr === "All") {
            this.setState({list: this.props.list});
            return;
        }
        this.setState({list: this.props.list.filter(item => item.currency === curr)});
    }
    showDealType(type) {
        this.setState({list: this.props.list.filter(item => item.dealType === type)});
    }

    render() {
        return (
            <div>
                <div className="d-flex flex-row justify-content-between">
                    <Btn name={"All"} click={this.showCurr} param={"All"}></Btn>
                    <Btn name={"BTC"} click={this.showCurr} param={"BTC"}></Btn>
                    <Btn name={"ETH"} click={this.showCurr} param={"ETH"}></Btn>
                    <Btn name={"USDT"} click={this.showCurr} param={"USDT"}></Btn>
                    <Btn name={"XRP"} click={this.showCurr} param={"XRP"}></Btn>
                    <Btn name={"BNB"} click={this.showCurr} param={"BNB"}></Btn>
                </div>
                <p/>
                <div className="d-flex flex-row justify-content-between">
                    <Btn name={"Long Buy"} click={this.showDealType} param={"Buy"}></Btn>
                    <Btn name={"Short Buy"} click={this.showDealType} param={"ShortBuy"}></Btn>
                    <Btn name={"Sell"} click={this.showDealType} param={"Sell"}></Btn>
                </div>
                <p/>
                {drawList(this.state.list)}
                <Modal key={"modal"} />
            </div>
        );
    }
}

class Btn extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <button className="btn btn-primary" onClick={() => this.props.click(this.props.param)}>
                {this.props.name}
            </button>
        );
    }
}

class Link extends React.Component {
    constructor(props) {
        super(props);
        
        this.handleClick = this.handleClick.bind(this);
    }
    
    handleClick() {
        const rate = document.getElementById("rate");
        const cb = document.getElementById("useLiveRate");
        cb.checked = false;
        rate.readOnly = false;
        
        let form = document.forms["modalForm"];
        document.getElementById("del").hidden = false;
        form.elements["modal-id"].value = this.props.id;
        form.elements["currency-select"].value = this.props.fullName;
        form.elements["amount"].value = this.props.amount;
        form.elements["rate"].value = this.props.rate;
        switch (this.props.dealType) {
            case "Buy":
                form.elements["type-select"].value = "Long Buy";
                break;
            case "ShortBuy":
                form.elements["type-select"].value = "Short Buy";
                break;
            case "Sell":
                form.elements["type-select"].value = "Sell";
                break;
        }
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
        this.checkHandle = this.checkHandle.bind(this)
    }
    
    handleSubmit(e) {
        e.preventDefault();
        const form = document.forms["modalForm"];
        const id = form.elements["modal-id"].value;
        const curr = form.elements["currency-select"].selectedIndex.toString();
        const amount = form.elements["amount"].value;
        const rate = form.elements["rate"].value;
        const type = form.elements["type-select"].selectedIndex.toString();
        const comm = form.elements["commentary"].value;
        
        if(id == 0) {
            createDeal(curr, amount, rate, type, comm);
        }
        else{
            updateDeal(curr, amount, rate, type, comm, id);
        }
    }
    
    handleDelete() {
        const id = document.forms["modalForm"].elements["modal-id"].value;
        deleteDeal(id);
    }
    
    checkHandle() {
        const cb = document.getElementById("useLiveRate");
        const rate = document.getElementById("rate");
        const currency = document.getElementById("currency-select");
        if(cb.checked === true) {
            rate.readOnly = true;
            rate.value = rates[currency.selectedIndex];
        }
        else {
            rate.readOnly = false;
        }
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
                                    <input id="modal-id" type="text" defaultValue="0" hidden/>
                                    <div className="col modal-custom d-flex flex-column align-items-center">
                                        <div className="form-group col-md-8">
                                            <label className="col-form-label font-weight-bold">Currency</label>
                                            <select id="currency-select" onChange={this.checkHandle}>
                                                <option>Bitcoin</option>
                                                <option>Ethereum</option>
                                                <option>Tether</option>
                                                <option>Ripple</option>
                                                <option>BNB</option>
                                            </select>
                                        </div>
                                        <div className="form-group col-md-8">
                                            <label className="col-form-label font-weight-bold">Amount</label>
                                            <input id="amount" type="number" step="any" required/>
                                        </div>
                                        <div className="form-group col-md-8">
                                            <label className="col-form-label font-weight-bold">Rate (USD for 1 cryptocoin)</label>
                                            <input id="rate" type="number" step="0.00001" required />
                                            <div className="d-flex">
                                                <input className="modalCheck" type="checkbox" id="useLiveRate" onChange={this.checkHandle}/>
                                                <div className="checkHint" >Live rate</div>
                                            </div>
                                        </div>
                                        <div className="form-group col-md-8">
                                            <label className="col-form-label font-weight-bold">Deal Type</label>
                                            <select id="type-select">
                                                <option>Long Buy</option>
                                                <option>Short Buy</option>
                                                <option>Sell</option>
                                            </select>
                                        </div>
                                        <div className="form-group col-md-8">
                                            <label className="col-form-label font-weight-bold">Commentary</label>
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