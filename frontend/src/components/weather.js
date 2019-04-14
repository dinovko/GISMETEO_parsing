import React from 'react';
import PropTypes from 'prop-types';
import { withStyles } from '@material-ui/core/styles';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';
import RService from '../services/service'

const CustomTableCell = withStyles(theme => ({
  head: {
    backgroundColor: theme.palette.common.black,
    color: theme.palette.common.white,
  },
  body: {
    fontSize: 14,
  },
}))(TableCell);

const styles = theme => ({
  root: {
    width: '100%',
    marginTop: theme.spacing.unit * 3,
    overflowX: 'auto',
  },
  table: {
    minWidth: 700,
  },
  row: {
    '&:nth-of-type(odd)': {
      backgroundColor: theme.palette.background.default,
    },
  },
});

class CustomizedTable extends React.Component {
  state = {
    weather:null,
    isLoading:false,
  }

  

render(){
  const {weather, isLoading} = this.state;
  const {classes, refer} = this.props;
  return (
    <Paper className={classes.root}>
      <Table className={classes.table}>
        <TableHead>
          <TableRow>
          <CustomTableCell>ДАТА</CustomTableCell>
            {refer.map((days) => (
                <CustomTableCell key={days.id}>{days.date}</CustomTableCell>
            ))
            }
          </TableRow>
        </TableHead>
        <TableBody>
            <TableRow className={classes.row}>
            <CustomTableCell component="th" scope="row">Температура `C</CustomTableCell>
            {refer.map((data) =>(
              <CustomTableCell key={data.id} align="right">
                {data.temp}
              </CustomTableCell>
              ))}
            </TableRow>
            <TableRow className={classes.row}>
            <CustomTableCell component="th" scope="row">Скорость ветра</CustomTableCell>
            {refer.map((data) =>(
              <CustomTableCell key={data.id} align="right">
                {data.windspeed}
              </CustomTableCell>
              ))}
            </TableRow>
            <TableRow className={classes.row}>
            <CustomTableCell component="th" scope="row">Давление</CustomTableCell>
            {refer.map((data) =>(
              <CustomTableCell key={data.id} align="right">
                {data.pressure}
              </CustomTableCell>
              ))}
            </TableRow>
            <TableRow className={classes.row}>
            <CustomTableCell component="th" scope="row">Влажность</CustomTableCell>
            {refer.map((data) =>(
              <CustomTableCell key={data.id} align="right">
                {data.humidity}
              </CustomTableCell>
              ))}
            </TableRow>
            <TableRow className={classes.row}>
            <CustomTableCell component="th" scope="row">Г/М поле</CustomTableCell>
            {refer.map((data) =>(
              <CustomTableCell key={data.id} align="right">
                {data.geomagneticf}
              </CustomTableCell>
              ))}
            </TableRow>
        </TableBody>
      </Table>
    </Paper>
  );
}
  
}

CustomizedTable.propTypes = {
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(CustomizedTable);