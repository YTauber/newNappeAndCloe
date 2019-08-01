import React, { Component } from 'react';
import produce from 'immer';
import axios from 'axios';
import NumberFormat from 'react-number-format';


export default class AddCustomer extends Component {
    state = {
        customer : {
            name : '',
            phone : '',
            address : '',
            email : '',
            taxExemt: false
        },
        message : ''
    }

    onInputChange = e => {
        const nextState = produce(this.state, draft => {
            draft.customer[e.target.name] = e.target.value;
        });
        this.setState(nextState);
    }

    checkExemt = () => {
        const nextState = produce(this.state, draft => {
            draft.customer.taxExemt = !draft.customer.taxExemt;
        });
        this.setState(nextState);
    }

    addCustomer = () => {

        const {name, phone, address, email, taxExemt} = this.state.customer;
        
        if (!name){
            this.setState({message : 'You didn\'t enter a valid name... :('})
        }
      
        else {

        axios.post( '/api/customer/addcustomer', {name, phone, address, email, taxExemt}).then(({data}) => {

                
                this.props.history.push(`/customer/${data.id}`);
            });
            

        }
       
    }
    render() {

        const {customer, message} = this.state;
        const {name, phone, address, email, taxExemt} = customer;
        const {onInputChange, addCustomer, checkExemt} = this;

        let messg = '';
        if (message) {
            messg = (<div style={{textAlign: 'center'}} className="alert alert-danger" role="alert">
            {message}
          </div>)
        }

        return (
            <div>
                <div className="container" style={{marginTop: '40px'}}>
                    <div className="row">
                        <div className="col-md-4 col-md-offset-4">
                            <div className="well">
                                {messg}
                                <input name="name" type="text" className="form-control" placeholder="Name" value={name} onChange={onInputChange} />
                                <br />
                                <NumberFormat className="form-control" placeholder="Phone" value={phone} onChange={onInputChange} name="phone" format="1 (###) ###-####" mask="_"/>
                                <br />
                                <input name="address" type="text" className="form-control" placeholder="Address" value={address} onChange={onInputChange} />
                                <br />
                                <input name="email" type="text" className="form-control" placeholder="Email" value={email} onChange={onInputChange} />
                                <br />
                                <h7><input type="checkBox" checked={taxExemt} onChange={checkExemt} /><label>Tax Exemt</label></h7>
                                <br/>
                                <button style={{marginTop: '10px'}} onClick={addCustomer} className="btn btn-primary btn-block">Add Customer</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
