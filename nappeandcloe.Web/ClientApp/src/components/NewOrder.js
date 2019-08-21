import React, { Component } from 'react';
import axios from 'axios';
import produce from 'immer';
import moment from 'moment';
import { format } from 'money-formatter';
import Calendar from 'react-calendar';
import InputNumeric from 'react-input-numeric';


export default class NewOrder extends Component {
    state = {
        order : {
            customerId : '',
            customer :{
                id : '',
                name :''
            },
            name : '',
            address : '',
            date : '',
            tax : '',
            deliveryCharge: '',
            discount : '',
            notes : '',
            liner : {
              quantity: '',
              cahrge: '',
              myCaharge: ''  
            },
            productViews : [],
            total : '',
            taxExemt : '',
            discuntAmount: '',

        },
        
        loading : true,
        message : ''

    }

    componentDidMount = () => {

        this.getDraft();
    }

    getDraft = () => {
        axios.get('/api/draft/getDraftOrder').then(({ data }) => {

            const nextState = produce(this.state, draft => {
                draft.order = data;
            });
            this.setState(nextState);

            this.getCustomer(data.customerId)
            console.log(data)
        });

    }

    getCustomer = (id) => {

        axios.get(`/api/customer/getCustomerbyid/${id}`).then(({ data }) => {

            const nextState = produce(this.state, draft => {
                draft.order.customer = data;
            });
            this.setState(nextState);
            console.log(data)
        });

    }

    addDraft = () => {

        const {customerId, name, date, tax, deliveryCharge, discount, notes, linerId, productViews, taxExemt, address} = this.state.order;
        if (date){
            axios.post(`/api/draft/addorderdraft`, {customerId, name, date, tax, deliveryCharge, discount, notes, linerId, productViews, taxExemt, address}).then(({data}) => this.setTotals(data))
        }
        else {
        axios.post(`/api/draft/addorderdraft`, {customerId, name, tax, deliveryCharge, discount, notes, linerId, productViews, taxExemt, address}).then(({data}) => this.setTotals(data))
        }
    }

    addLinerDraft = () => {

        const {quantity, cahrge, myCaharge} = this.state.order.liner;
        axios.post('/api/draft/addLinerDraft', {quantity, cahrge, myCaharge}).then(({data}) => this.setTotals(data));
    }

    setTotals = (data) => {
        const {discuntAmount, tax, total} = data;
        const nextState = produce(this.state, draft => {
            draft.order.discuntAmount = discuntAmount;
            draft.order.tax = tax;
            draft.order.total = total;
        });
        this.setState(nextState);
    } 

    onInputChange = (e) => {
      
            const nextState = produce(this.state, draft => {
                draft.order[e.target.name] = e.target.value;
            });
            this.setState(nextState,() => {this.addDraft() } );
                
    }

    onLinerChange = (e) => {
      
        const nextState = produce(this.state, draft => {
            draft.order.liner[e.target.name] = e.target.value;
        });
        this.setState(nextState,() => {this.addLinerDraft() } );
            
}


    viewCustomer = () => {
        this.props.history.push(`/customer/${this.state.order.customerId}`)
    }

    changeCustomer = () => {
        this.props.history.push(`/customers`)
    }

    changeDate = () => {
        this.props.history.push(`/`)
    }


    ViewProducts = () => {
        this.props.history.push(`/inventory`)
    }

    viewProduct = (id) => {
        this.props.history.push(`/viewProduct/${id}`)
    }

    startOver = () => {

        axios.post('/api/draft/startover').then(({data}) => {

            let order = data;
            order.notes = '';
            order.name = '';
            this.setState({order });
        });
    }

    priceChange = (e, id) => {
        if(e.target.value){
            const nextState = produce(this.state, draft => {

                draft.order.productViews.find(p => p.productSizeViews.some(s => s.id ===id)).productSizeViews.find(d => d.id === id).orderPrice = e.target.value
            });
            this.setState(nextState,() => {this.addDraft() } );
        }

        
             
    }

    amountChange = (e, id) => {

        if(e){
            const nextState = produce(this.state, draft => {

                draft.order.productViews.find(p => p.productSizeViews.some(s => s.id ===id)).productSizeViews.find(d => d.id === id).orderAmount = e
                
            });
            this.setState(nextState,() => {this.addDraft() } );
        }

        
             
    }

    
    remove = (id) => {
        const nextState = produce(this.state, draft => {

            draft.order.productViews.forEach(p => p.productSizeViews = p.productSizeViews.filter(s => s.id !== id));

        });
        this.setState(nextState,() => {this.addDraft() } );
    }

    checkExecmt = () => {
        const nextState = produce(this.state, draft => {
            draft.order.taxExemt = !draft.order.taxExemt;
        });
        this.setState(nextState,() => {this.addDraft() } );
    }


    addLiner = () => {
        if(this.state.order.date){
            const nextState = produce(this.state, draft => {
                draft.order.liner.quantity = draft.order.liner.quantity + 1;
            });
            this.setState(nextState,() => {this.addLinerDraft() } );
        }
    }

    minusLiner = () => {
        if (this.state.order.liner.quantity > 0){
            const nextState = produce(this.state, draft => {
                draft.order.liner.quantity = draft.order.liner.quantity - 1;
            });
            this.setState(nextState,() => {this.addLinerDraft() } );
        }
    }

    discountChange = (e) => {
        const nextState = produce(this.state, draft => {
            draft.order.discount = e
        });
        this.setState(nextState,() => {this.addDraft() } );
    }

    deliveryChargeChange = (e) => {
        const nextState = produce(this.state, draft => {
            draft.order.deliveryCharge = e
        });
        this.setState(nextState,() => {this.addDraft() } );
    }

    setLinerContent = () => {
        const nextState = produce(this.state, draft => {
            draft.showLiner = !draft.showLiner;
            draft.order.liner.quantity = 1;
            draft.order.liner.cahrge = 10;
            draft.order.liner.myCaharge = 10;
        });
        this.setState(nextState);
    }

    submit = () => {
        const {customer, name, date, productViews} = this.state.order;

        if (!date){
            this.setState({message : 'Please select a Date'});
        }
        else if (!customer){
            this.setState({message: 'Please select a Customer'})
        }
        else if (!name){
            this.setState({message: 'Please select an Event Name'})
        }
        else if (!productViews.length){
            this.setState({message: 'Please select Products'})
        }
        else{
            axios.post('/api/order/addOrder').then(() => {
                this.startOver();
                this.props.history.push('/');
            })
        }
    }

  
    render() {
 
        const {date, customer, liner, name, notes, productViews, tax, deliveryCharge, discount, total, discuntAmount, taxExemt, address} = this.state.order;
        const {onInputChange, viewCustomer, changeCustomer, startOver, ViewProducts, viewProduct, setLinerContent, onLinerChange,
            priceChange, changeDate, checkExecmt, discountChange, deliveryChargeChange,
              addLiner, minusLiner, submit, amountChange} = this;
              const {message} = this.state;


       let messg = '';
       if (message) {
           messg = (<div style={{textAlign: 'center'}} className="alert alert-danger" role="alert">
                        {message}
                   </div>)
          }


        let customerContent = '';
        if (customer) {
            customerContent = (
                <div>
                <div style={{textAlign: 'center'}}>
                    <div className="col-md-12 well" >
                            <h1 style={{cursor : 'pointer'}} onClick={viewCustomer}>{customer.name}</h1>
                            <button className="btn btn-link" onClick={changeCustomer}>Change Customer</button>
                    </div>
                    
                </div>
               
                </div>
            )
        }
        else {

           customerContent = (
                <button className="btn btn-info btn-block" onClick={changeCustomer}>Get Customer</button>
           ) 
        }

        let cal = '';
        if (date) {
            cal = (
                <div style={{width: '100%'}}>
                <Calendar
                  value={new Date(moment(date))}
                  calendarType="US"
                  minDate={new Date(moment(date))}
                  maxDate={new Date(moment(date))}
            />
                </div>
            )
        }
            else{
                cal = (
                    <button className="btn btn-info btn-block" onClick={changeDate}>Get Date</button>
               ) 
            }


        let linerContent = '';
        if (liner.quantity){
            linerContent = (
                        <tr>
                                  <td>
                                    <div className="well" style={{cursor: 'pointer', textAlign: 'center', margin: '5px'}}>
                                         <h5>Liners</h5>
                                    </div>
                                </td>
                                <td>
                                      <div className="col-md-2 col-md-offset-3" style={{cursor: 'pointer'}} onClick={minusLiner}>
                                          <h3 className="glyphicon glyphicon-menu-left"></h3>
                                    </div>
                                     <div className="col-md-2">
                                        <h3>{liner.quantity}</h3>
                                    </div>
                                    <div className="col-md-2" style={{cursor: 'pointer'}} onClick={addLiner}>
                                        <h3 className="glyphicon glyphicon-menu-right"></h3>
                                    </div>
                                </td>
                                <td>
                                    <div style={{textAlign:'center'}}>
                                      <input style={{marginLeft: '25%', width: '50%'}} placeholder="PricePer" name="cahrge" type="text" className="form-control" value={liner.cahrge} onChange={onLinerChange} />
                                      </div>
                                  </td>
                                <td>
                                    <div style={{textAlign:'center'}}>
                                        <input style={{marginLeft: '25%', width: '50%'}} placeholder="MyCharge" name="myCaharge" type="text" className="form-control" value={liner.myCaharge} onChange={onLinerChange} />
                                    </div>
                                </td>
                        </tr>
            )
        }
        else {
             linerContent = (
                        <tr>
                            <td colSpan={4}>
                                  <div className="col-md-2 col-md-offset-5" 
                                       style={{cursor: 'pointer', border: '1px solid', padding: '10px', borderRadius: '5px', marginTop: '10px'}}
                                       onClick={setLinerContent}>
                                    <h4>Add Liner</h4>
                                 </div>
                            </td>
                        </tr>
             )
        }
           
        let productsContent = '';
        if (productViews.length > 0){
            productsContent = (
                <div className='col-md-12' style={{marginTop: '25px'}}>
                    {productViews.filter(p => p.productSizeViews.length).map(p => <div className='col-md-12 well' key={p.id}>
                        <h1 style={{cursor : 'pointer'}} onClick={() => viewProduct(p.id)}>{p.name}</h1>
                        <div className="col-md-12" style={{marginTop: '15px'}}>
                    <table style={{textAlign: 'center'}} className="table table-striped">
                        <tr style={{textAlign: 'center'}}>
                             <th style={{textAlign:'center'}}>Remove</th>
                             <th style={{textAlign:'center'}}>Size</th>
                             <th style={{textAlign:'center'}}>Amount</th>
                             <th style={{textAlign:'center', width: '20%'}}>Your Price</th>
                        </tr>
                        {p.productSizeViews.map((d) => <tr key={d.productId} >
                            <td>
                                <div style={{cursor: 'pointer'}} onClick={() => this.remove(d.id)}>
                                    <h3 className="glyphicon glyphicon-remove"></h3>
                                </div>
                            </td>
                            <td>
                                <div  style={{cursor: 'pointer', textAlign: 'center', margin: '5px'}}>
                                    
                                            <h2>{d.size}</h2>
                                </div>
                            </td>
                            <td>
                                <InputNumeric
                                    value={d.orderAmount}
                                    onChange={(e) => amountChange(e, d.id)}
                                    min={d.minAvail}
                                />
                               <br />
                               {d.maxAvail ? <label style={{marginTop: 10, color: d.orderAmount > d.maxAvail ? 'red' : ''}}>{d.maxAvail} Available</label> : ''}
                            </td>
                            <td>
                                <div style={{textAlign:'center'}}>
                                    
                                     <input 
                                        style={{marginLeft: '25%', width: '50%'}}
                                        type="text" 
                                        className="form-control" 
                                        value={d.orderPrice} 
                                        onChange={(e) => priceChange(e, d.id)} />
                                </div>
                            </td>
                        </tr>)}
                        </table>
                        </div>
                        </div>
                       
                    )}
                    
                    <table style={{textAlign: 'center'}} className="table">
                        {linerContent}
                        <tr>
                            
                            <td colSpan={4}>
                                <h4 style={{marginTop: '15px'}}>Delivery Charge
                                <span style={{margin: 10}}>:</span>
                                <span style={{margin: 10}}>$</span>
                                <InputNumeric
                                    value={deliveryCharge}
                                    onChange={(e) => deliveryChargeChange(e)}
                                    min={0}
                                />
                                </h4>
                        </td>
                        </tr>
                        <tr>
                            <td colSpan={4}>
                                <h4 style={{marginTop: '15px'}}>Discount
                                <span style={{margin: 10}}>:</span>
                                    <InputNumeric
                                        value={discount}
                                        onChange={(e) => discountChange(e)}
                                        min={0}
                                    />
                                    <span style={{margin: 10}}>%</span>
                                    <strong>{format('USD', discuntAmount)}</strong>
                                </h4>
                        </td>
                        </tr>
                        <tr>
                            <td colSpan={4}>
                                <h4 style={{marginTop: '15px'}}>Tax: <strong>{format('USD', tax)}</strong>
                                <input style={{marginLeft: '10px'}} checked={taxExemt} type='checkBox' onChange={checkExecmt} /><small>Tax exempt</small></h4>
                            </td>
                        </tr>
                        <tr>
                            <td colSpan={4}><h3>Total: <strong>{format('USD', total)}</strong></h3></td>
                        </tr>
                        <tr>
                            <td colSpan={4}>
                                {messg}
                                <button onClick={submit} style={{marginTop: '20px'}} className="btn btn-success btn-block">Submit Order</button>
                            </td>
                        </tr>
                        </table>
                        </div>
                )
                
                        
          
        } 


        return (
            <div>
                <div className="container">
                <div className='row' style={{margin: 15}}>
                    <button onClick={() => this.props.history.goBack()} className='btn btn-sm btn-primary'>Back</button>                     
               </div>
                    <div className="row">
                        <div className="col-md-6 col-md-offset-3" style={{textAlign: 'center', marginTop: 20}}>
                            <button className="btn btn-link" onClick={startOver}><h4>Start Over</h4></button>
                        </div>
                        <div className="col-md-10 col-md-offset-1" style={{border: '1px solid', borderRadius: '5px', padding: '10px'}}>
                        <div className="col-md-6" style={{marginTop: '15px'}}>
                            {customerContent}
                            <div style={{marginTop: '15px'}}>
                                    <input type='text' name='name' className="form-control" value={name} placeholder="Event Name" onChange={onInputChange} />
                                </div>
                            
                            <div style={{marginTop: '15px'}}>
                                    <input type='text' name='address' className="form-control" value={address} placeholder="Event Address" onChange={onInputChange} />
                                </div>
                            </div>
                            <div className="col-md-6" style={{marginTop: '15px'}}>
                            {cal}
                            </div>
                            <div className="col-md-12" style={{marginTop: '25px'}}>
                                      <textarea rows="2" cols="100" name="notes" value={notes} onChange={onInputChange} placeholder="Notes..." className="form-control" />
                            </div>
                            <div className="col-md-12" style={{marginTop: '15px'}}>
                                <button className="btn btn-success btn-block btn-lg" onClick={ViewProducts}>Add Products</button>
                            </div>
                            <div>
                            {productsContent}
                           </div>
                       </div>
                        
                    </div>
                </div>
            </div>
        )
    }
}
