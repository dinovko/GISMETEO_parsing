import React from 'react'
import ReactDOM from 'react-dom'
import PropTypes from 'prop-types';
import { withStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';
import Input from '@material-ui/core/Input';
import OutlinedInput from '@material-ui/core/OutlinedInput';
import FilledInput from '@material-ui/core/FilledInput';
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import FormHelperText from '@material-ui/core/FormHelperText';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import CircularProgress from '@material-ui/core/CircularProgress';
import RService from '../services/service'
import Weather from './weather'

const styles = theme => ({
    button: {
      margin: theme.spacing.unit,
    },
    input: {
      display: 'none',
    },
    root: {
      display: 'flex',
      flexWrap: 'wrap',
    },
    formControl: {
      margin: theme.spacing.unit,
      minWidth: 120,
    },
    selectEmpty: {
      marginTop: theme.spacing.unit * 2,
    },
  });

class GetCity extends React.Component {
state = {
    data:null,
    age: '',
    name: 'hai',
    labelWidth: 0,
    cityList:"",
    selectedCity:"",
    isLoading: false,
}

    handleLoadCities = () => {
      this.setState({isLoading:true});
        const url = 'list/';
    RService.get(`${url}`)
      .then(response => {
        if(!response.isError) {
          this.setState({cityList: response.content, isLoading:false });
        }
      });
    }

    handleGetWeather = (id) => {
      const url = 'weather/load';
       RService.get(`${url}/${id}`)
        .then(response => {
          if(!response.isError) {
          this.setState({data: response.content, isLoading:false });
        }
      });
    }

    handleChange = event => {
        this.setState({ [event.target.name]: event.target.value });
    };

render(){
    const { classes } = this.props;
    const {cityList, isLoading, selectedCity, data} = this.state;
    return(
        <div>
          {
            isLoading &&
            <CircularProgress disableShrink />
          }
            <br/><FormControl className={classes.formControl}>
          <InputLabel shrink htmlFor="city-label-placeholder">
            город
          </InputLabel>
          <Select
            value={selectedCity}
            onChange={this.handleChange}
            input={<Input name="city" id="city-label-placeholder" />}
            displayEmpty
            name="selectedCity"
            className={classes.selectEmpty}
          >
            <MenuItem value="">
              <em>None</em>
            </MenuItem>
            {
                cityList && cityList.map((data) => (
                    <MenuItem key={data.id} value={data.id}>{data.cityName}</MenuItem>
                ))
            }
            
          </Select>
          <FormHelperText>выберите город</FormHelperText>
        </FormControl>
        <Button 
          variant="contained" 
          color="primary"
          onClick={this.handleLoadCities} 
          className={classes.button}>
          Загрузить список городов
        </Button>
        <Button 
          variant="contained" 
          color="primary"
          onClick={() => this.handleGetWeather(selectedCity)} 
          className={classes.button}>
          получить погоду
        </Button>
      </div>
    )
}

}

GetCity.propTypes = {
    classes: PropTypes.object.isRequired,
  };

export default withStyles(styles)(GetCity);