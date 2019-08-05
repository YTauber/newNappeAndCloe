import React, { Component } from 'react';
import { format } from 'money-formatter';
import axios from 'axios';
import produce from 'immer';
import Calendar from 'react-awesome-calendar';



export default class Product extends Component {
    state = {

        product: {
            id : '',
            name : '',
            price : '',
            notes : '',
            pictureName: '',
            productLabels: [],

            productSizeViews : [],
            calendarEvents : []
        },

        loading : true,
    }

    componentDidMount = () => {

        axios.get(`/api/product/getproductbyid/${this.props.match.params.id}`).then(({ data }) => {
            
            this.setState({ product: data, loading: false });
        });

    }


    reserve = () => {
        
        const {product} = this.state;
        const {id, name, price, pictureName, productSizeViews} = product;

        axios.post('/api/draft/addOrderDetailToDraft', {id, name, price, pictureName, productSizeViews: productSizeViews.filter(s => s.checked)}).then(() => {
            this.props.history.push(`/newOrder`)
        })
    }

    checkOrderId = (id) => {

        const nextState = produce(this.state, draft => {
            
            draft.product.productSizeViews.find(p => p.id === id).checked = !draft.product.productSizeViews.find(p => p.id === id).checked
        });
        this.setState(nextState);

    }

    edit = () => {

    }

    onChangeDate = () => {

    }

    render() {
        const {product, loading} = this.state;
        const {name, price, notes, pictureName, productLabels, productSizeViews, calendarEvents} = product;
        const {edit, reserve, checkOrderId, onChangeDate} = this

      
        let notesBox = '';
        if (notes){
            notesBox = (<div className="col-md-12" style={{textAlign: 'center', border: '1px solid', borderRadius: '5px', margin: '10px', padding: '10px'}}>
                            <p style={{whiteSpace: 'pre-line'}}>{notes}</p>
                        </div>)
        }

        let content = '';
        if (loading) {
            content = (<h1>loading</h1>)
        }
        else {
            content = (<div>
                <div className='container' style={{marginTop: 50}}>
                    <div className="row">
                        <div className="col-md-6 col-md-offset-3">
                            <div className="col-md-12">
                                <div className="col-md-4 col-md-offset-4">
                                     <img src={`/UploadedImages/${pictureName}`} alt='pic' className="img-responsive" style={{maxHeight: '200px', borderRadius: '50%'}} />
                                </div>
                                <div className="col-md-12" style={{textAlign: 'center'}}>
                                    <h2>{name}</h2>
                               </div>
                                <div className="row">
                                    <div className="col-md-6" style={{textAlign: 'center'}}>
                                        <h4>{format('USD', price)} each</h4>
                                        <h4>fooo</h4>
                                        <h4>fooo</h4>
                                    </div>
                                    <div className="col-md-6" style={{textAlign: 'center'}}>
                                        <h4>fooo</h4>
                                        <h4>fooo</h4>
                                        <h4>fooo</h4>
                                    </div>
                                </div>
                                {notesBox}
                                <div className="col-md-12" style={{textAlign: 'center'}}>
                                  {productLabels.map(l => <button style={{margin: '5px'}} key={l.label.id} className="btn">{l.label.name}</button>)}
                                </div>
                                <div className="col-md-12" style={{marginTop: '25px'}}>
                                    <table className="table">
                                        <thead>
                                            <tr>
                                              <th style={{textAlign: 'center'}}>Order</th>
                                              <th style={{textAlign: 'center'}}>Size</th>
                                              <th style={{textAlign: 'center'}}>Quantity</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {productSizeViews.map((s) => <tr key={s.id}>
                                                <td style={{textAlign: 'center'}}>
                                                    <input checked={s.checked} onClick={() => checkOrderId(s.id)} className='form-control' type='checkBox' />
                                                    {s.maxAvail ? <label>{s.maxAvail} Available</label> : ''}
                                                </td>
                                                <td style={{textAlign: 'center'}}><h3>{s.size}</h3></td>
                                                <td style={{textAlign: 'center'}}><h3>{s.quantity}</h3></td>
                                            </tr>)}
                                        </tbody>
                                    </table>
                                </div>
                                <div className="row">
                                    <div className="col-md-12" style={{marginTop: 25}}>
                                        <button onClick={reserve} className="btn btn-success btn-lg btn-block">Add to order</button>
                                     </div>
                                     <div className='col-md-12' style={{marginTop: 25}}>
                                        <Calendar
                                                    events={calendarEvents}
                                                    onChange={onChangeDate}
                                                />
                                    </div>
                                     <div className="col-md-12" style={{marginTop: 25}}>
                                        <div className="col-md-6 col-md-offset-3">
                                            <button onClick={edit} className="btn btn-info btn-block">Edit Product</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>





         
            </div>)
        }


        return (
            <div>
                {content}
            </div>
        )
    }
}
