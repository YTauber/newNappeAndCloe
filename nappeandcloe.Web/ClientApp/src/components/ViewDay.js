import React, { Component } from 'react';
import moment from 'moment';
import axios from 'axios';
import Calendar from 'react-calendar';

export default class ViewDay extends Component {
    state = {
        date : '',
        events : []
    }

    componentDidMount = () => {

        const {year, month, day} = this.props.match.params;
        const date = new Date(year, month, day);
        this.setState({date});

        axios.get(`/api/calendar/getCalendarEventsByDay/${date.getMonth() + 1}/${year}/${day}`).then(({data}) => {
            this.setState({events : data})
        })
    }

    addDate = () => {
        const {date} = this.state;

        axios.post(`/api/draft/addDateDraft`, {date}).then(() => {

            this.props.history.push(`/newOrder`)
        })
    }



    render() {
        const {date, events} = this.state;
        const {addDate} = this;

        return (
            <div>
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
                        <div style={{marginTop: 25}} className='col-md-12'>
                            {events.map((e) => <button className='btn btn-lg btn-block' style={{color: e.color}}>{e.title}</button>)}
                        </div>
                        <div style={{marginTop: 25}} className='col-md-12'>
                            <button className='btn btn-success btn-block' onClick={addDate}>Add to order</button>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
