import React, { Component } from 'react';
import axios from 'axios'

export default class Customer extends Component {
    state = {
        customer : {
            name : '',
            phone : '',
            address : '',
            email : '',
            id : '',
            taxExemt: false
        },

        loading: true
    }

    componentDidMount = () => {

        axios.get(`/api/customer/getCustomerbyid/${this.props.match.params.id}`).then(({ data }) => {
            if (!data.id){
                this.props.history.push('/customers')
            }
            this.setState({ customer: data, loading: false });
            
        });

    }

    addCustomer = () => {
        const {id} = this.state.customer;

        axios.post(`/api/draft/addcustomerdraft`, {id}).then(() => {

            this.props.history.push(`/newOrder`)
        })
    }

    edit = () => {

        const {id} = this.state.customer;
        this.props.history.push(`/addCustomer/${id}`);
    }

    render() {

        const {customer} = this.state;
        const {name, phone, address, email, taxExemt} = customer
        const {addCustomer, edit} = this;

        let taxExemtContent= '';
        if (taxExemt){
            taxExemtContent = (
                <div style={{margin: '25px'}}>
                     <strong>Tax Exempt</strong>
                </div>
            )
        }
//home earphone / phone-alt comment envelope
        return (
            <div>
                <div className='row' style={{margin: 15}}>
                    <button onClick={() => this.props.history.goBack()} className='btn btn-sm btn-primary'>Back</button>                     
               </div>
                <div className="container" style={{marginTop: '30px'}}>
                    <div className="row">
                        <div className="col-md-6 col-md-offset-3">
                            <div className="row">
                                <div className="col-md-12" style={{textAlign: 'center', padding:'20px', marginBottom: '20px', border: '1px solid', borderRadius: '5px'}}>
                                    <h1>{name}</h1>
                                </div>
                            </div>
                            
                            <div className="row">
                                <div className="col-md-9 col-md-offset-3">
                                    
                                    <h4><span className='glyphicon glyphicon-phone-alt' style={{margin: 15}}></span>{phone}</h4>
                                    <h4><span className='glyphicon glyphicon-home' style={{margin: 15}}></span>{address}</h4>
                                    <h4><span className='glyphicon glyphicon-envelope' style={{margin: 15}}></span>{email}</h4>
                                    {taxExemtContent}
                                    <br/>
                                </div>
                                <button className="btn btn-success btn-block btn-lg" onClick={addCustomer}>Add To Order</button>
                            </div>
                            <div className='row'>
                                <div className='col-md-6 col-md-offset-3' style={{textAlign: 'center', marginTop: 20}}>
                                    <button onClick={edit} className='btn btn-sm btn-info'>Edit</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
