import React, { Component } from 'react';
import moment from 'moment';
import axios from 'axios';
import Calendar from 'react-calendar';

export default class ViewDay extends Component {
    state = {
        date : '',
        day : {
            calendarEvents : [],
            orders : []
        }
    }

    componentDidMount = () => {

        const {year, month, day} = this.props.match.params;
        const date = new Date(year, month, day);
        this.setState({date});

        axios.get(`/api/calendar/getDay/${date.getMonth() + 1}/${year}/${day}`).then(({data}) => {
            this.setState({day : data})
        })
    }

    addDate = () => {
        const {date} = this.state;

        axios.post(`/api/draft/addDateDraft`, {date}).then(() => {

            this.props.history.push(`/newOrder`)
        })
    }

    viewCustomer = (id) => {
        this.props.history.push(`/customer/${id}`);
    }

    viewProduct = (id) => {
        this.props.history.push(`/viewProduct/${id}`)
    }



    render() {
        const {date, day} = this.state;
        const {calendarEvents, orders} = day;
        const {addDate, viewCustomer, viewProduct} = this;

        let ordersContent = ''
        if (orders.length){
            ordersContent = (
                <div>
<div style={{marginTop: 25, textAlign:'center', border: '1px solid black', borderRadius: '5px'}} className='col-md-12'>
                            <div className='col-md-12'>
                                <div style={{marginTop: 15}} className='col-md-11'>
                                    <button className='btn btn-block btn-sm btn-warning'>Print</button>
                                </div>
                                <div className='col-md-1' style={{marginTop: 15}}>
                                    <h4>{orders.length}</h4>
                                </div>
                            </div>
                            <div className='col-md-12'>
                                {orders.map((o) => 
                                <div className='row' style={{margin: 25, border: '1px solid black', borderRadius: '5px'}}>
                                    <h4>
                                        {o.name} for 
                                        <span style={{cursor : 'pointer'}} onClick={() => viewCustomer(o.customerId)}> {o.customer.name}</span>
                                    </h4>
                                    <hr />
                                    {o.productViews.map((p) => 
                                    <div className='col-md-12'>
                                        <div className="row well" onClick={() => viewProduct(p.id)} style={{cursor: 'pointer'}}>
                                            <div className="col-md-3" style={{margin: 5, textAlign: 'center'}}>
                                                    <img src={`/UploadedImages/${p.pictureName}`} alt="pic" className="img-responsive" style={{maxHeight: '75px', borderRadius: '50%'}} />
                                            </div>
                                            <div className="col-md-6" style={{margin: 5, textAlign: 'center', marginLeft: 80}}>
                                               <label>{p.name}</label>
                                                {p.productSizeViews.map((s) => <div>
                                                    <h5>{s.quantity} {s.size}</h5>
                                                </div>)}
                                            </div>
                                        </div>
                                     </div>)}
                                </div>)}
                            </div>
                       </div>
                </div>
            )
        }
        return (
            <div>
                <div className='col-md-3' style={{marginTop: 15}}>
                        <button onClick={() => this.props.history.goBack()} className='btn btn-sm btn-primary'>Back</button>                     
               </div>
                <div className='row'>
                    <div className='col-md-6 col-md-offset-3'>
                        <div style={{marginTop: 25}} className='col-md-12'>
                            <div className='col-md-offset-2'>
                                <div style={{width: '100%'}}>
                                    <Calendar
                                        value={date}
                                        calendarType="US"
                                        minDate={date}
                                        maxDate={date}
                                    />
                                </div>
                            </div>
                        </div>
                        <div>
                            {calendarEvents.map((e) => <button className='btn btn-lg btn-block' style={{color: e.color}}>{e.title}</button>)}
                        </div>
                        {ordersContent}
                        <div style={{marginTop: 25}} className='col-md-12'>
                            <button className='btn btn-success btn-block' onClick={addDate}>Add to order</button>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
