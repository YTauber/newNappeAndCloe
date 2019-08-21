import React, { Component } from 'react';
import produce from 'immer';
import axios from 'axios';
import NumberFormat from 'react-number-format';


export default class AddCustomer extends Component {
    state = {
        customer : {
            id:'',
            name : '',
            phone : '',
            address : '',
            email : '',
            taxExemt: false
        },
        message : '',
        hideButton: '',
        errorMessage : '',
        customerNames : []
    }

    componentDidMount = () => {
        if (this.props.match.params.id){

            axios.get(`/api/customer/getCustomerbyid/${this.props.match.params.id}`).then(({ data }) => {
            
                this.setState({ customer: data });
                
            });
           
        }

        axios.get('/api/customer/getAllCustomerNames').then(({data}) => {
            this.setState({customerNames: data})
        })
    }

    onInputChange = e => {

        const {customerNames} = this.state;

        const nextState = produce(this.state, draft => {
            draft.customer[e.target.name] = e.target.value;
            draft.errorMessage = e.target.name === 'name' && customerNames.some(c => c.toLowerCase() === e.target.value.toLowerCase())
            
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

        const {id, name, phone, address, email, taxExemt} = this.state.customer;
        
        if (!name){
            this.setState({message : 'You didn\'t enter a valid name... :('})
        }
      
        else {

            if(!id){
                axios.post( '/api/customer/addcustomer', {name, phone, address, email, taxExemt}).then(({data}) => {
                    this.props.history.push(`/customer/${data.id}`);
                });
            }
            else{
                axios.post( '/api/customer/updatecustomer', {id, name, phone, address, email, taxExemt}).then(({data}) => {
                    this.props.history.push(`/customer/${data.id}`);
                });
                
            }
            this.setState({hideButton: true});
        }
       
    }
    render() {

        const {customer, message, hideButton, errorMessage} = this.state;
        const {id, name, phone, address, email, taxExemt} = customer;
        const {onInputChange, addCustomer, checkExemt} = this;

        let messg = '';
        if (message) {
            messg = (<div style={{textAlign: 'center'}} className="alert alert-danger" role="alert">
            {message}
          </div>)
        }

        let buttonContent = '';
        if(!hideButton){
            if(!id){
                buttonContent = (
                    <button style={{marginTop: '10px'}} onClick={addCustomer} className="btn btn-primary btn-block">Add Customer</button>
                )
            }
            else{
                buttonContent = (
                    <button style={{marginTop: '10px'}} onClick={addCustomer} className="btn btn-info btn-block">Update Customer</button>
                )
            }
        }

        return (
            <div>
              <div className='row' style={{margin: 15}}>
                    <button onClick={() => this.props.history.goBack()} className='btn btn-sm btn-primary'>Back</button>                     
               </div> 
                <div className="container" style={{marginTop: '40px'}}>
                    <div className="row">
                        <div className="col-md-4 col-md-offset-4">
                            <div className="well">
                                {messg}
                                {errorMessage ? <label style={{color : 'red'}}>You already have a customer with this name</label> : ''}
                                <input name="name" type="text" className="form-control" placeholder="Name" value={name} onChange={onInputChange} />
                                <br />
                                <NumberFormat className="form-control" placeholder="Phone" value={phone} onChange={onInputChange} name="phone" format="1 (###) ###-####" mask="_"/>
                                <br />
                                <input name="address" type="text" className="form-control" placeholder="Address" value={address} onChange={onInputChange} />
                                <br />
                                <input name="email" type="text" className="form-control" placeholder="Email" value={email} onChange={onInputChange} />
                                <br />
                                <h7><input type="checkBox" checked={taxExemt} onChange={checkExemt} /><label>Tax Exempt</label></h7>
                                <br/>
                                {buttonContent}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
