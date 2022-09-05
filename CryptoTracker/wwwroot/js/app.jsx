fetch('http://localhost:5001/api/MainPage')
    .then(result => result.json())
    .then(result => {
        let table = result.map((item) => {
                return <tr key={item.shortName}>
                    <td>{item.name}</td>
                    <td>{item.shortName}</td>
                    <td>{item.value}</td>
                </tr>
            });

        ReactDOM.render(
            <JsonData table={table} />,
            document.getElementById("app")
        )
    });


class JsonData extends React.Component {
    constructor(props) {
        super(props);
    }
    
    render() {
        return(
            <div>
                <table className="table">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Short Name</th>
                            <th>Value</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.props.table}
                    </tbody>
                </table>
            </div>
        )
    }
}

