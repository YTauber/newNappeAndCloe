import React, { Component } from 'react';
import moment from 'moment';
import axios from 'axios';

export default class ViewDay extends Component {
    state = {
        date : ''
    }

    componentDidMount = () => {

        const {year, month, day} = this.props.match.params;
        const date = new Date(year, month, day);
        this.setState({date});
    }

    addDate = () => {
        const {date} = this.state;

        axios.post(`/api/draft/addDateDraft`, {date}).then(() => {

            this.props.history.push(`/newOrder`)
        })
    }



    render() {
        const {date} = this.state;
        const {addDate} = this;

        return (
            <div>
                <h1>{date.toString()}</h1>
                <button onClick={addDate}>Add to order</button>
                
            </div>
        )
    }
}
