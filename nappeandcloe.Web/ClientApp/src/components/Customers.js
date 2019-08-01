import React, { Component } from 'react';
import axios from 'axios';
import Loader from 'react-loader-spinner';
import { Link } from 'react-router-dom';


export default class Customers extends Component {
    state = {
        customers : [],
        searchContent : '',
        loading : true
    }

    componentDidMount = () => {

        this.setCustomers();

    }

    setCustomers = () => {
        axios.get('/api/customer/getAllCustomers').then(({ data }) => {
            
            this.setState({ customers: data });

            let {customers, searchContent} = this.state;

            customers = customers.filter(c => c.name.toLowerCase().includes(searchContent.toLowerCase()));

            this.setState({customers, loading: false});
        });
    }

    search = (e) => {

        this.setState({searchContent: e.target.value});
        this.setCustomers();
    }

    viewCustomer = (id) => {

        this.props.history.push(`/customer/${id}`)
    }

    render() {

        const {loading, customers, searchContent} = this.state;
        const {search, viewCustomer} = this;

        let content = "";
        if (loading){
            content = (
                <div className="container" style={{textAlign: 'center', marginTop: '100px'}}>
                    <Loader type="Puff" color="#00BFFF" height="100" width="100" />   
                    <h2>Loading..</h2>
                </div>
            )
        }
        else {
            content = (
                <div className="container" style={{marginTop: '75px'}}>
                    <div className="row">
                        <div className="col-md-4 col-md-offset-4">
                                <div style={{marginBottom: '15px'}}>
                                    <Link to='/addcustomer'>
                                        <button className="btn btn-block btn-primary">Add Customer</button>
                                    </Link>
                                </div>
                                <div className="row">
                                        <div className="col-md-12" style={{marginBottom: '15px'}}>
                                            <input type="text" placeholder="Search name..." className="form-control" onChange={search} value={searchContent} />
                                        </div>
                                    <div className="col-md-12">
                                        <div className="col-md-1 col-md-offset-11">
                                            <h4>{customers.length}</h4>
                                        </div>
                                    </div>
                                </div>

                            {customers.map((c) => <div onClick={() => viewCustomer(c.id)} key={c.id} className="well col-md-12" style={{marginTop: '10px', textAlign: 'center', cursor: 'pointer'}}>
                                    <h2>{c.name}</h2>
                            </div>)}
                        </div>
                    </div>
                </div>
            )
        }
        return (
            <div>
                {content}
            </div>
        )
    }
}
