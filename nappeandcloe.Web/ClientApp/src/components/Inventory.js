import React, { Component } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import produce from 'immer'
import Loader from 'react-loader-spinner';



export default class Inventory extends Component {
    state={
        inventory: {
            productViews : [],
            labels : [],
            sizes: [],
        },

        productDisplay: [],
        searchContent: '',
        searchIds: [],
        searchSizeIds: [],

        loading: true,
        errorMessage : '',
        editSizesMode: ''
    }

    

    componentDidMount = () => {

       axios.get('/api/product/getInventory').then(({data}) => {
           this.setState({inventory: data, loading: false, productDisplay: data.productViews})
       })
    }

    tagClicked = (id) => {
        const{searchIds} = this.state;
        const nextState = produce(this.state, draft => {

            if (searchIds.some(i => i === id)){
                draft.searchIds = draft.searchIds.filter(s => s !== id)
            }
            else{
                draft.searchIds.push(id);
            }
        });
        this.setState(nextState, () => {this.setFilters()});
    }

    sizeClicked = (id) => {
        if (!this.state.editSizesMode){
            const{searchSizeIds} = this.state;
        const nextState = produce(this.state, draft => {

            if (searchSizeIds.some(i => i === id)){
                draft.searchSizeIds = draft.searchSizeIds.filter(s => s !== id)
            }
            else{
                draft.searchSizeIds.push(id);
            }
        });
        this.setState(nextState, () => {this.setFilters()});
        }
       
    }

    search = (e) => {

        this.setState({searchContent: e.target.value}, () => {this.setFilters()})
    }

    viewProduct = (id) => {
        this.props.history.push(`/viewProduct/${id}`)
    }

    updateSizes = () => {
        const {sizes} = this.state.inventory;
        const distinctSizes = [...new Set(sizes.map(s => s.name))];
        if (sizes.length > distinctSizes.length){
            this.setState({errorMessage: 'You shouldn\'t name two sizes the same'})
        }
        else{
            axios.post(`/api/product/updateSizes`, {sizes});
            this.setState({editSizesMode: false, errorMessage: ''})
        }
    }

    editSizes = () => {
        this.setState({editSizesMode: true})
    }

    changeSize = (e, id) => {
        const nextState = produce(this.state, draft => {
            draft.inventory.sizes.find(s => s.id === id).name = e.target.value;
        });
        this.setState(nextState);
    }

    clearFilters = () => {
        this.setState({searchIds : [], searchSizeIds: [], searchContent: ''}, () => {this.setFilters()})
    }

    setFilters = () => {
        let{searchContent, searchIds, searchSizeIds, inventory, productDisplay} = this.state;
        let{productViews} = inventory;
        
            productDisplay = productViews.filter(p => p.name.toLowerCase().includes(searchContent.toLowerCase()));

            if (searchIds.length){
                productDisplay = productDisplay.filter(p => searchIds.every(s => p.productLabels.map(l => l.labelId).includes(s)))
            }
            if (searchSizeIds.length){
                productDisplay = productDisplay.filter(p => searchSizeIds.every(s => p.productSizes.map(l => l.sizeId).includes(s)))
            }
            this.setState({productDisplay})
    }

    render() {

        const {searchContent, searchIds, loading, searchSizeIds, editSizesMode, inventory, productDisplay, errorMessage} = this.state;
        const{labels, sizes} = inventory
        const {tagClicked, search, viewProduct, sizeClicked, changeSize, updateSizes, editSizes, clearFilters} = this;

        
        const tagStyle = {
            margin : 5, 
            size: 10,
            cursor : 'pointer'
        }

        const divStyle = {
            marginTop : 25
        }

        let messg = '';
       if (errorMessage) {
           messg = (<div style={{textAlign: 'center'}} className="alert alert-danger" role="alert">
                        {errorMessage}
                   </div>)
          }

       
        let content = '';
        if (loading){
            content = (
                <div className="container" style={{textAlign: 'center', marginTop: '100px'}}>
                    <Loader
                type="Puff"
                color="#00BFFF"
                height="100"
                width="100"
            />   
            <h2>Loading..</h2>
                </div>
            )
        }
        else{
            content = (
                <div>
                <div className="container">
                    <div className='row'>
                    <div className='col-md-3'>
                            <div style={{marginTop: 25, textAlign: 'center'}}>
                                <label>Sizes</label>
                            </div>
                            <hr />
                        {sizes.map(s => <div key={s.id} onClick={() => {sizeClicked(s.id)}} style={{border: '1px solid', margin: 15, padding: 5, textAlign: 'center', borderRadius: '5px', cursor: 'pointer'}}>
                                {editSizesMode ? 
                            <input style={{margin: 5, width: '90%'}} type='text' value={s.name} className='form-control' placeholder='Size' onChange={(e) => {changeSize(e, s.id)}} />
                                : 
                            searchSizeIds.some(i => i === s.id) ? <h2>{s.name}</h2> : <h4>{s.name}</h4>
                            }
                            </div>)}
                            <div className='col-md-12'>
                                {messg}
                            </div>
                            <div className='col-md-12'>
                                {!editSizesMode ? 
                                    <button className='btn btn-sm btn-block btn-warning' onClick={editSizes}>Edit Sizes</button>
                                        :
                                    <button className='btn btn-sm btn-block btn-success' onClick={updateSizes}>Update Sizes</button>
                                }
                            </div>
                    </div>
                    <div className="col-md-6">
                        <div className='row'>
                            <div style={divStyle} className="col-md-12">
                                 <Link to='/addproduct'>
                                       <button className="btn btn-block btn-primary">Add Product</button>
                                 </Link>
                             </div>
                            <div>
                                <div className="col-md-12" style={divStyle}>
                                    <input type="text" placeholder="Search name..." className="form-control" onChange={search} value={searchContent} />
                                </div>
                                <div style={{marginTop: 10}} className="col-md-12">
                                    <div style={{display:'inline', float:'left'}} className='col-md-3'>
                                        {searchIds.length || searchContent || searchSizeIds.length ?
                                        <button onClick={clearFilters} className='btn btn-sm btn-block btn-success'>Clear Filters</button>
                                        : ''}
                                    </div>
                                    <div className="col-md-1 col-md-offset-8">
                                            <h4>{productDisplay.length}</h4>
                                    </div>
                                </div>
                            </div>
                       </div>
                       
                   
                    <div style={{textAlign: 'center'}}>
                            {productDisplay.map((p) => (
                                <div key={p.id} className="row well" onClick={() => viewProduct(p.id)} style={{cursor: 'pointer'}}>
                                    <div className="col-md-3" style={{margin: 5, textAlign: 'center'}}>
                                            <img src={`/UploadedImages/${p.pictureName}`} alt="pic" className="img-responsive" style={{maxHeight: '75px', borderRadius: '50%'}} />
                                    </div>
                                    <div className="col-md-6 offset-md-4" style={{margin: 5, textAlign: 'center', marginLeft: 80}}>
                                
                                            <h3>{p.name}</h3>
                                    </div>
                                </div>
                            )
                            )}
                    </div>
                </div>
                <div className='col-md-3'>
                  <div>
                        <div style={{marginTop: 25, textAlign: 'center'}} className="col-md-12">
                            <div style={{textAlign: 'center'}}>
                                <label>Labels</label>
                            </div>
                            <hr />
                                {labels.map(l => <h1 style={tagStyle} onClick={() => {tagClicked(l.id)}} className={searchIds.some(i => i === l.id) ? "btn btn-primary" : "btn btn-info"} key={l.id}>{l.name}</h1>)}
                        </div>
                   </div>
                </div>
                </div>
                </div>
                </div>)
        }

        return (
            <div>
                <div className='row' style={{margin: 15}}>
                    <button onClick={() => this.props.history.goBack()} className='btn btn-sm btn-primary'>Back</button>                     
               </div>
                {content}
            </div>
        )
    }
}
