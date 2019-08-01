import React, { Component } from 'react';
import { format } from 'money-formatter';
import axios from 'axios';
import produce from 'immer';

const customStyles = {
    content : {
      top                   : '50%',
      left                  : '60%',
      right                 : 'auto',
      bottom                : 'auto',
      marginRight           : '-50%',
      transform             : 'translate(-50%, -50%)'
    }
  };


export default class Product extends Component {
    state = {

        product: {
            id : '',
            name : '',
            price : '',
            notes : '',
            pictureName: '',
            productLabels: [],
            productSizes: []
        },

        loading : true,

        orderIds : []
    }

    componentDidMount = () => {

        axios.get(`/api/product/getproductbyid/${this.props.match.params.id}`).then(({ data }) => {
            
            this.setState({ product: data, loading: false });
            console.log(this.state.product)
        });

    }


    reserve = () => {
        
        const {product, orderIds} = this.state;
        const {id, name, price, pictureName} = product;
        axios.post('/api/draft/addOrderDetailToDraft', {orderDetails: orderIds, product : {id, name, price, pictureName}}).then(() => {
            this.props.history.push(`/newOrder`)
        })
    }

    checkOrderId = (id) => {

        const nextState = produce(this.state, draft => {
            if (draft.orderIds.some(i => i=== id)){
                draft.orderIds = draft.orderIds.filter(i => i !== id)
            }
            else {
                draft.orderIds.push(id)
            }
        });
        this.setState(nextState);

    }

    edit = () => {


    }

    render() {
        const {product, loading, orderIds} = this.state;
        const {name, price, notes, pictureName, productLabels, productSizes} = product;
        const {edit, reserve, checkOrderId} = this

      
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
                                            {productSizes.map((s) => <tr key={s.id}>
                                                <td style={{textAlign: 'center'}}>
                                                    <input checked={orderIds.some(i => i === s.id)} onClick={() => checkOrderId(s.id)} className='form-control' type='checkBox' />
                                                </td>
                                                <td style={{textAlign: 'center'}}>{s.size.name}</td>
                                                <td style={{textAlign: 'center'}}>{s.quantity}</td>
                                            </tr>)}
                                        </tbody>
                                    </table>
                                </div>
                                <div className="row">
                                    <div className="col-md-12" style={{marginTop: 25}}>
                                        <button onClick={reserve} className="btn btn-success btn-lg btn-block">Add to order</button>
                                     </div>
                                     <div className="col-md-12" style={{marginTop: 25}}>
                                        <div className="col-md-6 col-md-offset-3">
                                            <button onClick={edit} className="btn btn-info btn-block">Edit</button>
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
