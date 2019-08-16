import React, { Component } from 'react';
import axios from 'axios';
import produce from 'immer';

export default class ViewOrder extends Component {
    state  = {
        order: {
            id:'',
            name:'',
            address:'',
            date: '',
            deliveryCharge: '',
            discount: '',
            notes : '',
            total:'',
            taxExemt:'',
            tax:'',
            discountAmount:'',

            productViews:[],
            payments:[],
            customer: '',
            customerId:''
        }
    }

    componentDidMount = () => {
        axios.get(`/api/order/getOrderById/${this.props.match.params.id}`).then(({ data }) => {
            
            this.setState({ order: data });
        });

    }


    render() {
        return (
            <div>
                <div className='row' style={{margin: 15}}>
                    <button onClick={() => this.props.history.goBack()} className='btn btn-sm btn-primary'>Back</button>                     
               </div>
                <h1>{this.state.order.name} for {this.state.order.customer.name}</h1>
            </div>
        )
    }
}
